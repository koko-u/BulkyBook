using BulkyBook.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.BusinessCore.Services;

public interface ICategoriesService
{
    Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

    Task<CategoryViewModel> CreateNewCategory(CreateCategoryViewModel createCategory);
}