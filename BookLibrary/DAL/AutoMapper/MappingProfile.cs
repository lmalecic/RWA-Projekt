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
            CreateMap<Models.Book, DTO.BookUpdateDto>().ReverseMap();

            CreateMap<Models.User, DTO.UserDto>().ReverseMap();

            CreateMap<Models.UserReservation, DTO.UserReservationDto>();
            CreateMap<DTO.UserReservationDto, Models.UserReservation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Models.UserReservation, DTO.UserReservationUpdateDto>().ReverseMap();

            CreateMap<Models.UserReview, DTO.UserReviewDto>();
            CreateMap<DTO.UserReviewDto, Models.UserReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Models.UserReview, DTO.UserReviewUpdateDto>().ReverseMap();

            CreateMap<Models.Genre, DTO.GenreDto>();
            CreateMap<DTO.GenreDto, Models.Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Models.Genre, DTO.GenreUpdateDto>().ReverseMap();

            CreateMap<Models.Location, DTO.LocationDto>();
            CreateMap<DTO.LocationDto, Models.Location>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Models.Location, DTO.LocationUpdateDto>().ReverseMap();

            CreateMap<Models.BookLocation, DTO.BookLocationDto>().ReverseMap();
        }
    }
}
