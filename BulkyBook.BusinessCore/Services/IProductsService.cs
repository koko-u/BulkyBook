using BulkyBook.Presentation.ViewModels;

namespace BulkyBook.BusinessCore.Services;

public interface IProductsService
{
    Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
}