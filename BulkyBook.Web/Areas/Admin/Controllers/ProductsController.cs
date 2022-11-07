using BulkyBook.BusinessCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductsController : Controller
{
    private readonly IProductsService _productsService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
    {
        _productsService = productsService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productsService.GetAllProductsAsync();
        return View(products);
    }
}