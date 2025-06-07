using AutoMapper;
using Azure;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

    public class SearchResult<T>
    {
        public int Count;
        public int Page;
        public int Total;
        public IEnumerable<T> Results;

        public SearchResult(int count, int page, int total, IEnumerable<T> results)
        {
            this.Count = count;
            this.Page = page;
            this.Total = total;
            this.Results = results;
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

        public Book Get(int id)
        {
            var existing = _context.Books
                .Include(x => x.BookLocations)
                .Include(x => x.UserReservations)
                .Include(x => x.UserReviews)
                .Include(x => x.Genre)
                .FirstOrDefault(x => x.Id == id);

            if (existing == null) {
                _logService.Log($"Could not find book with id={id}", 1);
                throw new FileNotFoundException($"Book with id {id} does not exist.");
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

        public Book Update(int id, IUpdateDto updateDto)
        {
            var existing = this.Get(id);

            _mapper.Map(updateDto, existing);
            _context.SaveChanges();
            _logService.Log($"Updated book with id={id}", 0);

            return existing;
        }

        public Book? Delete(int id)
        {
            if (!this.Exists(id)) {
                return null;
            }

            var entity = this.Get(id);

            // Check if the book is used in any reservation or review
            var isUsedInReservations = entity.UserReservations.Count != 0;
            var isUsedInReviews = entity.UserReviews.Count != 0;
            var isUsedInLocations = entity.BookLocations.Count != 0;

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
            return _context.Books
                .Include(x => x.BookLocations)
                .Include(x => x.UserReservations)
                .Include(x => x.UserReviews)
                .Include(x => x.Genre)
                .AsEnumerable();
        }

        public SearchResult<Book> Search(BookSearchParams searchParams)
        {
            if (searchParams.Page < 1 || searchParams.Count < 1)
            {
                _logService.Log($"Could not search books; page and count must be positive integers.", 1);
                throw new BadHttpRequestException("Page and count must be positive integers.");
            }

            var query = _context.Books
                .Include(x => x.BookLocations)
                .Include(x => x.UserReservations)
                .Include(x => x.UserReviews)
                .Include(x => x.Genre)
                .AsQueryable();

            if (searchParams.GenreId != null)
                query = query.Where(b => b.GenreId == searchParams.GenreId);

            if (!string.IsNullOrWhiteSpace(searchParams.Name))
                query = query.Where(b => b.Name.Contains(searchParams.Name));

            if (!string.IsNullOrWhiteSpace(searchParams.Author))
                query = query.Where(b => b.Author.Contains(searchParams.Author));

            if (!string.IsNullOrWhiteSpace(searchParams.Description))
                query = query.Where(b => b.Description != null ?
                    b.Description.Contains(searchParams.Description) :
                    false);

            var total = query.Count();

            var books = query
                .Skip((searchParams.Page - 1) * searchParams.Count)
                .Take(searchParams.Count)
                .AsEnumerable();

            _logService.Log($"Searched books (name: {searchParams.Name}, author: {searchParams.Author}, page: {searchParams.Page}, count: {searchParams.Count})", 0);

            return new (searchParams.Count, searchParams.Page, total, books);
        }

        public bool Exists(int id)
        {
            return _context.Books.Any(x => x.Id == id);
        }
    }
}
