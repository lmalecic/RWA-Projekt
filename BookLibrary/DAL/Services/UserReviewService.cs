using AutoMapper;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public UserReview Update(int id, IUpdateDto updateDto)
        {
            if (!this.Exists(id))
                throw new FileNotFoundException($"Review with id={id} not found.");

            var review = this.Get(id);

            _mapper.Map(updateDto, review);
            _context.SaveChanges();

            return review;
        }
    }
}
