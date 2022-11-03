using AutoMapper;
using BulkyBook.Persistence.Data;
using BulkyBook.Persistence.Models;
using BulkyBook.Presentation.Result;
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
        var categories = await _dbContext.Categories.OrderBy(cat => cat.DisplayOrder).ToListAsync();
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

    public async Task<CategoryViewModel?> GetSingleCategoryByIdAsync(Guid id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            return null;
        }

        return _mapper.Map<CategoryViewModel>(category);
    }

    public async Task<ResponseData<CategoryViewModel>> UpdateCategory(
        EditCategoryViewModel editCategory)
    {
        var targetCategory = await _dbContext.Categories.FindAsync(editCategory.Id);
        if (targetCategory == null)
        {
            return ResponseData.Error<CategoryViewModel>(
                $"Category with id: {editCategory.Id} is not found.");
        }

        // set update name
        if (!string.IsNullOrEmpty(editCategory.Name))
        {
            targetCategory.Name = editCategory.Name;
        }

        // set display order
        if (editCategory.UpOrDownCount > 0)
        {
            // [Old Order]
            //  NameA    /1/
            //  NameB    /2/   <- +2 Up
            //  NameC    /3/
            //  NameD    /4/   <- Target Category
            //
            // [Result Order]
            //  NameA    /1/
            //  NameD    /1.1/
            //  NameB    /2/
            //  NameC    /3/
            //
            // so, you must get NameA and NameB's Display Orders
            //
            // reversed order indexes
            //  NameA    /1/                2
            //  NameB    /2/   <- +2 Up     1
            //  NameC    /3/                0
            //  NameD    /4/   <- target
            //
            var prevCategories =
                await _dbContext.Categories
                    .Where(cat => cat.DisplayOrder < targetCategory.DisplayOrder)
                    .OrderByDescending(cat => cat.DisplayOrder)
                    .ToArrayAsync();
            var range = (editCategory.UpOrDownCount - 1)..(editCategory.UpOrDownCount + 1);
            var prevCategory = prevCategories[range];
            if (prevCategory.Length == 0)
            {
                // target category is the top order
                // do nothing
            }

            if (prevCategory.Length == 1)
            {
                // target category's display order to make top.
                targetCategory.DisplayOrder = HierarchyId.GetRoot()
                    .GetDescendant(null, prevCategory[0].DisplayOrder);
            }

            if (prevCategory.Length >= 2)
            {
                // target category's order to make between prevCategory[1] and prevCategory[0]
                // prevCategory is reversed.
                targetCategory.DisplayOrder = HierarchyId.GetRoot()
                    .GetDescendant(prevCategory[1].DisplayOrder, prevCategory[0].DisplayOrder);
            }
        }

        if (editCategory.UpOrDownCount < 0)
        {
            // [Old Order]
            //  NameA    /1/   <- target category
            //  NameB    /2/                          0
            //  NameC    /3/   <- (-2) down           1
            //  NameD    /4/                          2
            //
            // [Result Order]
            //  NameB    /2/
            //  NameC    /3/
            //  NameA    /3.1/
            //  NameD    /4/
            //
            // so, you must get NameC and NameD's Display Orders

            //var nextCategory =
            //    await _dbContext.Categories
            //        .Where(cat => cat.DisplayOrder > targetCategory.DisplayOrder)
            //        .OrderBy(cat => cat.DisplayOrder)
            //        .Skip(-editCategory.UpOrDownCount - 1)
            //        .Take(2)
            //        .ToListAsync();
            var nextCategories =
                await _dbContext.Categories
                    .Where(cat => cat.DisplayOrder > targetCategory.DisplayOrder)
                    .OrderBy(cat => cat.DisplayOrder)
                    .ToArrayAsync();
            var range = (-editCategory.UpOrDownCount - 1)..(-editCategory.UpOrDownCount + 1);
            var nextCategory =
                nextCategories[range];

            if (nextCategory.Length == 0)
            {
                // target category is the bottom
                // do nothing
            }

            if (nextCategory.Length == 1)
            {
                // target category's order to make last
                targetCategory.DisplayOrder = HierarchyId.GetRoot()
                    .GetDescendant(nextCategory[0].DisplayOrder, null);
            }

            if (nextCategory.Length >= 2)
            {
                // target category's order to make between nextCategory[0] and nextCategory[1]
                targetCategory.DisplayOrder = HierarchyId.GetRoot()
                    .GetDescendant(nextCategory[0].DisplayOrder, nextCategory[1].DisplayOrder);
            }
        }

        await _dbContext.SaveChangesAsync();

        return ResponseData.Ok(_mapper.Map<CategoryViewModel>(targetCategory));
    }

    private async Task<HierarchyId> GetNextOfLastDisplayOrderAsync()
    {
        var lastCategory = await _dbContext.Categories.OrderByDescending(x => x.DisplayOrder)
            .FirstOrDefaultAsync();
        if (lastCategory == null)
        {
            return HierarchyId.GetRoot().GetDescendant(null, null);
        }

        return HierarchyId.GetRoot().GetDescendant(lastCategory.DisplayOrder, null);
    }
}