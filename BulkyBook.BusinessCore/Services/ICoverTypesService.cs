using BulkyBook.Presentation.Result;
using BulkyBook.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.BusinessCore.Services;

public interface ICoverTypesService
{
    Task<IEnumerable<CoverTypeViewModel>> GetAllCoverTypesAsync();

    Task<ResponseData<CoverTypeViewModel>> CreateNewCoverTypeAsync(
        CreateCoverTypeViewModel createCoverType);

    Task<ResponseData<CoverTypeViewModel>> GetSingleCoverTypeByIdAsync(Guid id);

    Task<ResponseData<CoverTypeViewModel>> UpdateCoverTypeAsync(
        EditCoverTypeViewModel editCoverType);

    Task<ResponseData> DeleteCoverTypeByIdAsync(Guid id);
}