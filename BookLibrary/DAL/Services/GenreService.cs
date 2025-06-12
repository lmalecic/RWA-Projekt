using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class GenreService : IEntityService<Genre>
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public GenreService(BookLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Genre Get(int id)
        {
            var entity = _context.Genres.FirstOrDefault(x => x.Id == id);
            return entity != null ? entity
                : throw new FileNotFoundException($"Genre with id {id} does not exist.");
        }

        public Genre Create(Genre entity)
        {
            _context.Genres.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public Genre Update(Genre entity)
        {
            var existing = this.Get(entity.Id);

            _mapper.Map(entity, existing);
            _context.SaveChanges();

            return existing;
        }

        public Genre? Delete(int id)
        {
            if (!this.Exists(id)) {
                return null;
            }

            var entity = this.Get(id);
            var isUsedInBooks = _context.Books.Any(r => r.GenreId == entity.Id);
            if (isUsedInBooks) {
                throw new BadHttpRequestException($"Could not delete genre because it is used in books.");
            }

            _context.Genres.Remove(entity);
            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<Genre> GetAll()
        {
            return _context.Genres.AsEnumerable();
        }

        public bool Exists(int id)
        {
            return _context.Genres.Any(x => x.Id == id);
        }
    }
}
