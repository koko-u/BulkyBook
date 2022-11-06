using AutoMapper;
using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Abby.Web.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;

        [BindProperty]
        public CategoryViewModel DeletionCategoryViewModel { get; set; } = new();

        public DeleteModel(ICategoriesService categoriesService, IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var category = await _categoriesService.GetSingleCategoryByIdAsync(id);
            if (category == null)
            {
                TempData[TempDataKeys.FailureMessage] = $"The category with id[{id}] is not found.";
                return RedirectToPage("Index");
            }

            this.DeletionCategoryViewModel = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var deleted =
                await _categoriesService.DeleteCategoryByIdAsync(this.DeletionCategoryViewModel.Id);
            if (deleted.IsSuccess)
            {
                TempData[TempDataKeys.SuccessMessage] =
                    $"The category [{DeletionCategoryViewModel.Name}] has been deleted.";
            }
            else
            {
                TempData[TempDataKeys.FailureMessage] = deleted.ErrorMessages[0];
            }

            return RedirectToPage("Index");
        }
    }
}