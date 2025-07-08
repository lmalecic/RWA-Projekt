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
    public class UserService : IEntityService<Models.User>
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public UserService(BookLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public User Get(int id)
        {
            var entity = _context.Users
                .Include(u => u.UserReservations)
                    .ThenInclude(ur => ur.Book)
                .Include(u => u.UserReviews)
                    .ThenInclude(ur => ur.Book)
                .FirstOrDefault(x => x.Id == id);

            return entity != null ? entity
                : throw new FileNotFoundException($"User with id {id} does not exist.");
        }

        public User Get(string username)
        {
            var entity = _context.Users
                .Include(u => u.UserReservations)
                    .ThenInclude(ur => ur.Book)
                .Include(u => u.UserReviews)
                    .ThenInclude(ur => ur.Book)
                .FirstOrDefault(x => x.Username == username);

            return entity != null ? entity
                : throw new FileNotFoundException($"User with username {username} does not exist.");
        }

        public User Create(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public User Update(User entity)
        {
            var existing = this.Get(entity.Id);

            _mapper.Map(entity, existing);
            _context.SaveChanges();

            return existing;
        }

        public User? Delete(int id)
        {
            if (!this.Exists(id)) {
                return null;
            }

            var entity = this.Get(id);
            var isUsedInReservations = entity.UserReservations.Count > 0;
            if (isUsedInReservations) {
                throw new BadHttpRequestException($"Could not delete user because it is used in reservations.");
            }

            var isUsedInReviews = entity.UserReviews.Count > 0;
            if (isUsedInReviews) {
                throw new BadHttpRequestException($"Could not delete user because it is used in reviews.");
            }

            _context.Users.Remove(entity);
            _context.SaveChanges();

            return entity;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .Include(u => u.UserReservations)
                    .ThenInclude(ur => ur.Book)
                .Include(u => u.UserReviews)
                    .ThenInclude(ur => ur.Book)
                .AsEnumerable();
        }

        public bool Exists(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }
    }
}
