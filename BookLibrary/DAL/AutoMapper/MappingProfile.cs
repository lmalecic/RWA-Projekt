using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;

namespace DAL.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Book, DTO.BookDto>();
            CreateMap<DTO.BookDto, Models.Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Models.Book, DTO.BookPatchDto>().ReverseMap();
            CreateMap<Models.Book, DTO.BookUpdateDto>().ReverseMap();

            CreateMap<Models.User, DTO.UserDto>().ReverseMap();
            CreateMap<Models.UserReservation, DTO.UserReservationDto>().ReverseMap();
            CreateMap<Models.UserReview, DTO.UserReviewDto>().ReverseMap();

            CreateMap<Models.Genre, DTO.GenreDto>();
            CreateMap<DTO.GenreDto, Models.Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Models.Location, DTO.LocationDto>().ReverseMap();
        }
    }
}
