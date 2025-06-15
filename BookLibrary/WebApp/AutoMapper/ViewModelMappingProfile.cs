using AutoMapper;
using DAL.Services;

namespace WebApp.AutoMapper
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile() {
            CreateMap<DAL.Models.Book, ViewModels.BookViewModel>();
            CreateMap<ViewModels.BookViewModel, DAL.Models.Book>()
                .ForMember(b => b.Id, opt => opt.Ignore());
            CreateMap<DAL.Models.Book, ViewModels.BookCreateViewModel>();
            CreateMap<ViewModels.BookCreateViewModel, DAL.Models.Book>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<DAL.Models.Genre, ViewModels.GenreViewModel>();
            CreateMap<ViewModels.GenreViewModel, DAL.Models.Genre>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<DAL.Models.User, ViewModels.UserLoginViewModel>().ReverseMap();
            CreateMap<DAL.Models.User, ViewModels.UserViewModel>();
            CreateMap<ViewModels.UserViewModel, DAL.Models.User>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<SearchResult<DAL.Models.Book>, SearchResult<ViewModels.BookViewModel>>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
