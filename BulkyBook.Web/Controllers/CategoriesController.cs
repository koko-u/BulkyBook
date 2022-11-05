using AutoMapper;
using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoriesService categoriesService, IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
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

            var created = await _categoriesService.CreateNewCategoryAsync(createCategory);
            return RedirectToAction(nameof(Index));
        }

        // GET /Categories/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoriesService.GetSingleCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<EditCategoryViewModel>(category));
        }

        // POST /Categories/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditCategoryViewModel editCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(editCategory);
            }

            var response = await _categoriesService.UpdateCategoryAsync(editCategory);
            if (response.IsFailure)
            {
                ModelState.AddModelError(nameof(EditCategoryViewModel)
                                       , response.ErrorMessages.First());
                return View(editCategory);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET /Categories/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoriesService.GetSingleCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<CategoryViewModel>(category));
        }

        // POST /Categories/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var response = await _categoriesService.DeleteCategoryByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}