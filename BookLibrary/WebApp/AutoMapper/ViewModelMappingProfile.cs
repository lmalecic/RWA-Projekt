using AutoMapper;
using DAL.Services;

namespace WebApp.AutoMapper
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile() {
            // Book mappings
            CreateMap<DAL.Models.Book, Models.BookViewModel>()
                .ForMember(b => b.GenreId, opt => opt.MapFrom(src => src.Genre.Id));
            CreateMap<Models.BookViewModel, DAL.Models.Book>();
            CreateMap<DAL.Models.Book, Models.BookCreateViewModel>();
            CreateMap<Models.BookCreateViewModel, DAL.Models.Book>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Basic mappings
            CreateMap<DAL.Models.Genre, Models.GenreViewModel>().ReverseMap();
            CreateMap<DAL.Models.Location, Models.LocationViewModel>().ReverseMap();
            CreateMap<DAL.Models.User, Models.UserViewModel>().ReverseMap();

            // User related mappings
            CreateMap<DAL.Models.User, Models.UserLoginViewModel>().ReverseMap();
            CreateMap<DAL.Models.User, Models.UserRegisterViewModel>();
            CreateMap<Models.UserRegisterViewModel, DAL.Models.User>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            // Search results mapping
            CreateMap<SearchResult<DAL.Models.Book>, SearchResult<Models.BookViewModel>>().ReverseMap();

            // Reservation mappings
            CreateMap<DAL.Models.UserReservation, Models.ReservationEditViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date ?? DateTime.Now));
            
            CreateMap<Models.ReservationEditViewModel, DAL.Models.UserReservation>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));

            CreateMap<DAL.Models.UserReservation, Models.ReservationCreateViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.AvailableBooks, opt => opt.Ignore());
            
            CreateMap<Models.ReservationCreateViewModel, DAL.Models.UserReservation>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            // User Reservation ViewModel mapping
            CreateMap<DAL.Models.UserReservation, Models.UserReservationViewModel>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));
            CreateMap<Models.UserReservationViewModel, DAL.Models.UserReservation>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));

            // Review mappings
            CreateMap<DAL.Models.UserReview, Models.ReviewEditViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));
            
            CreateMap<Models.ReviewEditViewModel, DAL.Models.UserReview>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));

            CreateMap<DAL.Models.UserReview, Models.ReviewCreateViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.AvailableBooks, opt => opt.Ignore());
            
            CreateMap<Models.ReviewCreateViewModel, DAL.Models.UserReview>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            // User Review ViewModel mapping
            CreateMap<DAL.Models.UserReview, Models.UserReviewViewModel>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));
            CreateMap<Models.UserReviewViewModel, DAL.Models.UserReview>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));
        }
    }
}
