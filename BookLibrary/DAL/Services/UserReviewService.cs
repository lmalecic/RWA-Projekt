using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DAL.Services
{
    public class UserReviewService : IEntityService<UserReview>
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public UserReviewService(BookLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserReview Create(UserReview entity)
        {
            if (entity == null)
                throw new BadHttpRequestException(nameof(entity));

            _context.UserReviews.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public UserReview? Delete(int id)
        {
            var review = _context.UserReviews.Find(id);
            if (review == null)
                throw new FileNotFoundException($"Review with id={id} not found.");

            _context.UserReviews.Remove(review);
            _context.SaveChanges();
            return review;
        }

        public bool Exists(int id)
        {
            return _context.UserReviews.Any(r => r.Id == id);
        }

        public UserReview Get(int id)
        {
            var review = _context.UserReviews
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);

            if (review == null)
                throw new FileNotFoundException($"Review with id={id} not found.");

            return review;
        }

        public IEnumerable<UserReview> GetAll()
        {
            return _context.UserReviews
                .AsEnumerable();
        }

        public UserReview Update(UserReview entity)
        {
            var existing = this.Get(entity.Id);

            _mapper.Map(entity, existing);
            _context.SaveChanges();

            return existing;
        }
    }
}
