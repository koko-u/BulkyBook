using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        // GET /Categories/Index
        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET /Categories/Create
        public IActionResult Create()
        {
            return View(new CreateCategoryViewModel());
        }

        // POST /Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel createCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(createCategory);
            }

            var created = await _categoriesService.CreateNewCategory(createCategory);
            return RedirectToAction(nameof(Index));
        }
    }
}