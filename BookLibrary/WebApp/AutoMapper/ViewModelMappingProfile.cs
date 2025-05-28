using AutoMapper;

namespace WebApp.AutoMapper
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile() {
            CreateMap<DAL.Models.Book, ViewModels.BookViewModel>();
            CreateMap<ViewModels.BookViewModel, DAL.Models.Book>()
                .ForMember(b => b.Id, opt => opt.Ignore());
        }
    }
}
