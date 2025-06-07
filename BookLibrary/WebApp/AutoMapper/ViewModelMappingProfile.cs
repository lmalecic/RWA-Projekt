using AutoMapper;

namespace WebApp.AutoMapper
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile() {
            CreateMap<DAL.Models.Book, ViewModels.BookViewModel>();
            CreateMap<ViewModels.BookViewModel, DAL.Models.Book>()
                .ForMember(b => b.Id, opt => opt.Ignore());

            CreateMap<DAL.Models.User, ViewModels.UserLoginViewModel>().ReverseMap();
            CreateMap<DAL.Models.User, ViewModels.UserViewModel>();
            CreateMap<ViewModels.UserViewModel, DAL.Models.User>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
