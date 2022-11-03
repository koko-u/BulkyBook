using AutoMapper;
using BulkyBook.Persistence.Data;
using BulkyBook.Persistence.Models;
using BulkyBook.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.BusinessCore.Services;

public class CategoriesService : ICategoriesService
{
    private readonly BulkyBookDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoriesService(BulkyBookDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        return _mapper.Map<List<CategoryViewModel>>(categories);
    }

    public async Task<CategoryViewModel> CreateNewCategory(CreateCategoryViewModel createCategory)
    {
        var category = _mapper.Map<Category>(createCategory);
        category.DisplayOrder = await GetNextOfLastDisplayOrderAsync();

        await _dbContext.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CategoryViewModel>(category);
    }

    private async Task<HierarchyId> GetNextOfLastDisplayOrderAsync()
    {
        var lastCategory = await _dbContext.Categories.OrderByDescending(x => x.DisplayOrder).FirstOrDefaultAsync();
        if (lastCategory == null)
        {
            return HierarchyId.GetRoot().GetDescendant(null, null);
        }

        return HierarchyId.GetRoot().GetDescendant(lastCategory.DisplayOrder, null);
    }
}