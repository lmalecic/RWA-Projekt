using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class LocationService : IEntityService<Location>
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public LocationService(BookLibraryContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public Location Get(int id)
        {
            var entity = _context.Locations
                .Include(x => x.BookLocations)
                .FirstOrDefault(x => x.Id == id);
            if (entity == null) {
                throw new FileNotFoundException($"Location with id {id} does not exist.");
            }

            return entity;
        }

        public Location Create(Location entity)
        {
            _context.Locations.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public Location Update(Location entity)
        {
            var existing = this.Get(entity.Id);

            _mapper.Map(entity, existing);
            _context.SaveChanges();

            return existing;
        }

        public Location? Delete(int id)
        {
            if (!this.Exists(id)) {
                return null;
            }

            var entity = this.Get(id);
            var hasBooks = entity.BookLocations.Count != 0;
            if (hasBooks) {
                throw new BadHttpRequestException($"Could not delete genre because it is used in books.");
            }

            _context.Locations.Remove(entity);
            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<Location> GetAll()
        {
            return _context.Locations.AsEnumerable();
        }

        public bool Exists(int id)
        {
            return _context.Locations.Any(x => x.Id == id);
        }
    }
}
