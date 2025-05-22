using AutoMapper;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class BookLocationService : IAssociationService<BookLocation>
    {
        private readonly BookLibraryContext _context;
        private readonly IEntityService<Book> _bookService;
        private readonly IEntityService<Location> _locationService;
        private readonly LogService _logService;

        public BookLocationService(BookLibraryContext context, LogService logService, IEntityService<Book> bookService, IEntityService<Location> locationService)
        {
            _context = context;
            _logService = logService;
            _bookService = bookService;
            _locationService = locationService;
        }

        public IEnumerable<BookLocation> GetAll()
        {
            return _context.BookLocations
                .AsEnumerable();
        }

        public BookLocation Create(int id1, int id2)
        {
            var book = _bookService.Get(id1);
            var location = _locationService.Get(id2);

            var assocation = new BookLocation
            {
                BookId = book.Id,
                LocationId = location.Id
            };

            _context.BookLocations.Add(assocation);
            _context.SaveChanges();

            _logService.Log($"Book with id {id1} was added to location with id {id2}.", 0);
            return assocation;
        }

        public BookLocation? Delete(int id1, int id2)
        {
            if (!this.Exists(id1, id2)) {
                return null;
            }

            var association = _context.BookLocations.First(x => x.BookId == id1 && x.LocationId == id2);
            _context.BookLocations.Remove(association);
            _context.SaveChanges();

            _logService.Log($"Book with id {id1} was removed from location with id {id2}.", 0);
            return association;
        }

        public bool Exists(int id1, int id2)
        {
            return _context.BookLocations.Any(x => x.BookId == id1 && x.LocationId == id2);
        }
    }
}
