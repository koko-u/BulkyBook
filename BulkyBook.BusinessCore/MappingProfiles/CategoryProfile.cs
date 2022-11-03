using AutoMapper;
using BulkyBook.Persistence.Models;
using BulkyBook.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.BusinessCore.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryViewModel>()
            .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder.ToString()));
        CreateMap<CreateCategoryViewModel, Category>();
    }
}