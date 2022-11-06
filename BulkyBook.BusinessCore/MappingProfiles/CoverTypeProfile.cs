using AutoMapper;
using BulkyBook.Persistence.Models;
using BulkyBook.Presentation.ViewModels;

namespace BulkyBook.BusinessCore.MappingProfiles;

public class CoverTypeProfile : Profile
{
    public CoverTypeProfile()
    {
        CreateMap<CoverType, CoverTypeViewModel>();
        CreateMap<CreateCoverTypeViewModel, CoverType>();
        CreateMap<EditCoverTypeViewModel, CoverType>();
        CreateMap<CoverTypeViewModel, EditCoverTypeViewModel>();
    }
}