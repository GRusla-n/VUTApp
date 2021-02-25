using AutoMapper;
using VUTApp.Dtos;
using VUTApp.Models;

namespace VUTApp.Profiles
{
    public class DTOProfile : Profile
    {
        public DTOProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>()
                .ForMember(x => x.Image, options => options.Ignore());
        }        
    }
}
