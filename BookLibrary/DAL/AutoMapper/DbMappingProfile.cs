using AutoMapper;
using DAL.Models;

namespace DAL.AutoMapper
{
    public class DbMappingProfile : Profile
    {
        public DbMappingProfile()
        {
            CreateMap<Book, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Genre, opt => opt.Ignore())
                .ForMember(dest => dest.BookLocations, opt => opt.Ignore())
                .ForMember(dest => dest.UserReservations, opt => opt.Ignore())
                .ForMember(dest => dest.UserReviews, opt => opt.Ignore());

            CreateMap<Genre, Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            CreateMap<Location, Location>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookLocations, opt => opt.Ignore());

            CreateMap<BookLocation, BookLocation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<User, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserReviews, opt => opt.Ignore())
                .ForMember(dest => dest.UserReservations, opt => opt.Ignore());

            CreateMap<UserReservation, UserReservation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UserReview, UserReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
