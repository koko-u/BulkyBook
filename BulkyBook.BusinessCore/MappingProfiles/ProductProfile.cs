using AutoMapper;
using BulkyBook.Persistence.Models;
using BulkyBook.Presentation.ViewModels;

namespace BulkyBook.BusinessCore.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CoverTypeName, opt => opt.MapFrom(src => src.CoverType.Name));
    }
}