using AutoMapper;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
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
    }
}
