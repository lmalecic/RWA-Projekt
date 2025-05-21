using AutoMapper;
using Azure;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public struct BookSearchParams()
    {
        public int Count = 10;
        public int Page = 1;
        public string? Name;
        public string? Author;
        public string? Description;
        public int? GenreId;
    }

    // Generic class candidate: SearchResult<Book>
    public class BookSearchResult
    {
        public int Count;
        public int Page;
        public int Total;
        public IEnumerable<Book> Results;

        public BookSearchResult(int count, int page, int total, IEnumerable<Book> results)
        {
            Count = count;
            Page = page;
            Total = total;
            Results = results;
        }
    }

    public class BookService : IEntityService<Book>
    {
        private readonly BookLibraryContext _context;
        private readonly LogService _logService;
        private readonly IMapper _mapper;

        public BookService(BookLibraryContext context, LogService logService, IMapper mapper)
        {
            _context = context;
            _logService = logService;
            _mapper = mapper;
        }

        public Book? Get(int id)
        {
            var existing = _context.Books.FirstOrDefault(x => x.Id == id);
            if (existing == null) {
                _logService.Log($"Could not find book with id={id}", 1);
                return null;
            }

            _logService.Log($"Retrieved book with id={id}", 0);
            return existing;
        }

        public Book Create(Book entity)
        {
            _context.Books.Add(entity);
            _context.SaveChanges();

            _logService.Log($"Created book with id={entity.Id}", 0);
            return entity;
        }

        public Book Update(int id, object updateDto)
        {
            if (updateDto is not BookUpdateDto) {
                throw new BadHttpRequestException($"Invalid update object type; expected BookUpdateDto.");
            }

            var existing = Get(id);
            if (existing == null) {
                throw new BadHttpRequestException($"Could not update book; not found.");
            }

            _mapper.Map(updateDto, existing);
            _context.SaveChanges();

            _logService.Log($"Updated book with id={id}", 0);

            return existing;
        }

        public Book? Delete(int id)
        {
            var entity = Get(id);
            if (entity == null)
                return null;

            // Check if the book is used in any reservation or review
            var isUsedInReservations = _context.UserReservations.Any(r => r.BookId == entity.Id);
            var isUsedInReviews = _context.UserReviews.Any(r => r.BookId == entity.Id);
            var isUsedInLocations = _context.BookLocations.Any(r => r.BookId == entity.Id);

            if (isUsedInReservations || isUsedInReviews || isUsedInLocations) {
                _logService.Log($"Could not delete book with id={entity.Id}; it is used in reservations, reviews, or locations.", 1);
                throw new BadHttpRequestException($"Could not delete book because it is used in reservations, reviews, or locations.");
            }

            _context.Books.Remove(entity);
            _context.SaveChanges();

            _logService.Log($"Deleted book with id={entity.Id}", 0);

            return entity;
        }

        public IEnumerable<Book> GetAll()
        {
            _logService.Log($"Retrieved all books.", 0);
            return _context.Books.AsEnumerable();
        }

        public BookSearchResult Search(BookSearchParams searchParams)
        {
            if (searchParams.Page < 1 || searchParams.Count < 1)
            {
                _logService.Log($"Could not search books; page and count must be positive integers.", 1);
                throw new BadHttpRequestException("Page and count must be positive integers.");
            }

            var query = _context.Books.AsQueryable();

            if (searchParams.GenreId != null)
                query = query.Where(b => b.GenreId == searchParams.GenreId);

            if (!string.IsNullOrWhiteSpace(searchParams.Name))
                query = query.Where(b => b.Name.Contains(searchParams.Name));

            if (!string.IsNullOrWhiteSpace(searchParams.Author))
                query = query.Where(b => b.Author.Contains(searchParams.Author));

            if (!string.IsNullOrWhiteSpace(searchParams.Description))
                query = query.Where(b => b.Description != null ? b.Description.Contains(searchParams.Description) : false);

            var total = query.Count();

            var books = query
                .Skip(((searchParams.Page) - 1) * searchParams.Count)
                .Take(searchParams.Count)
                .ToList();

            _logService.Log($"Searched books (name: {searchParams.Name}, author: {searchParams.Author}, page: {searchParams.Page}, count: {searchParams.Count})", 0);

            return new BookSearchResult(searchParams.Count, searchParams.Page, total, books);
        }
    }
}
