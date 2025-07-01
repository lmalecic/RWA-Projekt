using AutoMapper;
using DAL.Services;

namespace WebApp.AutoMapper
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile() {
            CreateMap<DAL.Models.Book, Models.BookViewModel>()
                .ForMember(b => b.GenreId, opt => opt.MapFrom(src => src.Genre.Id));
            CreateMap<Models.BookViewModel, DAL.Models.Book>()
                .ForMember(b => b.Id, opt => opt.Ignore());
            CreateMap<DAL.Models.Book, Models.BookCreateViewModel>();
            CreateMap<Models.BookCreateViewModel, DAL.Models.Book>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<DAL.Models.Genre, Models.GenreViewModel>();
            CreateMap<Models.GenreViewModel, DAL.Models.Genre>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<DAL.Models.User, Models.UserLoginViewModel>().ReverseMap();
            CreateMap<DAL.Models.User, Models.UserViewModel>();
            CreateMap<Models.UserViewModel, DAL.Models.User>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<SearchResult<DAL.Models.Book>, SearchResult<Models.BookViewModel>>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
