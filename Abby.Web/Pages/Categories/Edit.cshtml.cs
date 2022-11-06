using AutoMapper;
using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Abby.Web.Pages.Categories;

public class EditModel : PageModel
{
    private readonly ICategoriesService _categoriesService;
    private readonly IMapper _mapper;

    [BindProperty]
    public EditCategoryViewModel EditCategory { get; set; } = new();

    public EditModel(ICategoriesService categoriesService, IMapper mapper)
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

        this.EditCategory = _mapper.Map<EditCategoryViewModel>(category);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var updated = await _categoriesService.UpdateCategoryAsync(this.EditCategory);
        if (updated.IsSuccess)
        {
            TempData[TempDataKeys.SuccessMessage] = $"The category [{updated.Value.Name}] has been updated.";
        }
        else
        {
            TempData[TempDataKeys.FailureMessage] = updated.ErrorMessages[0];
        }

        return RedirectToPage("Index");
    }
}