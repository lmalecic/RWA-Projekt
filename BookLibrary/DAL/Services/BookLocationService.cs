using AutoMapper;
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

        public BookLocation Create(BookLocation entity)
        {
            if (!_bookService.Exists(entity.BookId) || !_locationService.Exists(entity.LocationId))
                throw new BadHttpRequestException($"Book with id {entity.BookId} or Location with id {entity.LocationId} does not exist.");

            _context.BookLocations.Add(entity);
            _context.SaveChanges();

            _logService.Log($"Book with id {entity.BookId} was added to location with id {entity.LocationId}.", 0);
            return entity;
        }

        public BookLocation? Delete(BookLocation entity)
        {
            if (!this.Exists(entity)) {
                return null;
            }

            var association = _context.BookLocations.First(x => x.BookId == entity.BookId && x.LocationId == entity.LocationId);
            _context.BookLocations.Remove(association);
            _context.SaveChanges();

            _logService.Log($"Book with id {entity.BookId} was removed from location with id {entity.LocationId}.", 0);
            return association;
        }

        public bool Exists(BookLocation entity)
        {
            return _context.BookLocations.Any(x => x.BookId == entity.BookId && x.LocationId == entity.LocationId);
        }
    }
}
