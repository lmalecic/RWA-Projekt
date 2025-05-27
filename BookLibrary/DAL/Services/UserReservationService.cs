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
    public class UserReservationService : IEntityService<UserReservation>
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public UserReservationService(BookLibraryContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        public UserReservation Create(UserReservation entity)
        {
            if (entity == null)
                throw new BadHttpRequestException(nameof(entity));

            _context.UserReservations.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public UserReservation? Delete(int id)
        {
            var reservation = _context.UserReservations.Find(id);
            if (reservation == null)
                throw new FileNotFoundException($"Reservation with id={id} not found.");

            _context.UserReservations.Remove(reservation);
            _context.SaveChanges();
            return reservation;
        }

        public bool Exists(int id)
        {
            return _context.UserReservations.Any(r => r.Id == id);
        }

        public UserReservation Get(int id)
        {
            var reservation = _context.UserReservations
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                throw new FileNotFoundException($"Reservation with id={id} not found.");

            return reservation;
        }

        public IEnumerable<UserReservation> GetAll()
        {
            return _context.UserReservations
                .Include(r => r.Book)
                .Include(r => r.User)
                .ToList();
        }

        public UserReservation Update(int id, IUpdateDto updateDto)
        {
            if (!this.Exists(id))
                throw new FileNotFoundException($"Reservation with id={id} not found.");

            var reservation = this.Get(id);

            _mapper.Map(updateDto, reservation);
            _context.SaveChanges();

            return reservation;
        }
    }
}
