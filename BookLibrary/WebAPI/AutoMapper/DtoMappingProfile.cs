using AutoMapper;
using DAL.Models;

namespace WebAPI.AutoMapper
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<Book, DTO.BookDto>();
            CreateMap<DTO.BookDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Genre, opt => opt.Ignore())
                .ForMember(dest => dest.BookLocations, opt => opt.Ignore())
                .ForMember(dest => dest.UserReservations, opt => opt.Ignore())
                .ForMember(dest => dest.UserReviews, opt => opt.Ignore());
            CreateMap<Book, DTO.BookUpdateDto>();
            CreateMap<DTO.BookUpdateDto, Book>()
                .ForMember(dest => dest.Genre, opt => opt.Ignore())
                .ForMember(dest => dest.BookLocations, opt => opt.Ignore())
                .ForMember(dest => dest.UserReservations, opt => opt.Ignore())
                .ForMember(dest => dest.UserReviews, opt => opt.Ignore());

            CreateMap<User, DTO.UserDto>();
            CreateMap<DTO.UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserReviews, opt => opt.Ignore())
                .ForMember(dest => dest.UserReservations, opt => opt.Ignore());

            CreateMap<UserReservation, DTO.UserReservationDto>();
            CreateMap<DTO.UserReservationDto, UserReservation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<UserReservation, DTO.UserReservationUpdateDto>();
            CreateMap<DTO.UserReservationUpdateDto, UserReservation>()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UserReview, DTO.UserReviewDto>();
            CreateMap<DTO.UserReviewDto, UserReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<UserReview, DTO.UserReviewUpdateDto>();
            CreateMap<DTO.UserReviewUpdateDto, UserReview>()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Genre, DTO.GenreDto>();
            CreateMap<DTO.GenreDto, Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore());
            CreateMap<Genre, DTO.GenreUpdateDto>();
            CreateMap<DTO.GenreUpdateDto, Genre>()
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            CreateMap<Location, DTO.LocationDto>();
            CreateMap<DTO.LocationDto, Location>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookLocations, opt => opt.Ignore());
            CreateMap<Location, DTO.LocationUpdateDto>();
            CreateMap<DTO.LocationUpdateDto, Location>()
                .ForMember(dest => dest.BookLocations, opt => opt.Ignore());

            CreateMap<BookLocation, DTO.BookLocationDto>();
            CreateMap<DTO.BookLocationDto, BookLocation>()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());
        }
    }
}
