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

            CreateMap<Models.User, DTO.UserDto>().ReverseMap();
            CreateMap<Models.UserReservation, DTO.UserReservationDto>().ReverseMap();
            CreateMap<Models.UserReview, DTO.UserReviewDto>().ReverseMap();
        }
    }
}
