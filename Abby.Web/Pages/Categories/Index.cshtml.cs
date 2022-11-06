using AutoMapper;
using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Abby.Web.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ICategoriesService _categoriesService;
    private readonly IMapper _mapper;

    public IEnumerable<CategoryViewModel>? Categories;

    public IndexModel(ICategoriesService categoriesService, IMapper mapper)
    {
        _categoriesService = categoriesService;
        _mapper = mapper;
    }

    public async Task OnGetAsync()
    {
        this.Categories = await _categoriesService.GetAllCategoriesAsync();
    }
}