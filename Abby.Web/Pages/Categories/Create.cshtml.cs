using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Abby.Web.Pages.Categories;

public class CreateModel : PageModel
{
    private readonly ICategoriesService _categoriesService;

    [BindProperty]
    public CreateCategoryViewModel CreateCategory { get; set; } = new();

    public CreateModel(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var created = await _categoriesService.CreateNewCategoryAsync(CreateCategory);

        return RedirectToPage("Index");
    }
}