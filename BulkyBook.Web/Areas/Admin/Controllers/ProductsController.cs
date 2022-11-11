using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductsController : Controller
{
    private readonly IProductsService _productsService;
    private readonly ICategoriesService _categoriesService;
    private readonly ICoverTypesService _coverTypesService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductsService productsService
        , ICategoriesService categoriesService
        , ICoverTypesService coverTypesService
        , ILogger<ProductsController> logger)
    {
        _productsService = productsService;
        _categoriesService = categoriesService;
        _coverTypesService = coverTypesService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productsService.GetAllProductsAsync();
        return View(products);
    }


    // GET /Admin/Products/Create
    public async Task<IActionResult> Create()
    {
        var createProduct = new CreateProductViewModel();
        var categories = await _categoriesService.GetAllCategoriesAsync();
        createProduct.Categories = categories.Select(category => new SelectListItem
            { Text = category.Name, Value = category.Id.ToString() });

        var coverTypes = await _coverTypesService.GetAllCoverTypesAsync();
        createProduct.CoverTypes = coverTypes.Select(coverType => new SelectListItem
            { Text = coverType.Name, Value = coverType.Id.ToString() });

        return View(createProduct);
    }
}