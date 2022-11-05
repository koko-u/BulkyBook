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

    public async Task<CategoryViewModel> CreateNewCategoryAsync(CreateCategoryViewModel createCategory)
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

    public async Task<ResponseData<CategoryViewModel>> UpdateCategoryAsync(
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
            targetCategory.DisplayOrder =
                await UpByCount(targetCategory, editCategory.UpOrDownCount);
        }

        if (editCategory.UpOrDownCount < 0)
        {
            targetCategory.DisplayOrder =
                await DownByCount(targetCategory, -editCategory.UpOrDownCount);
        }

        await _dbContext.SaveChangesAsync();

        return ResponseData.Ok(_mapper.Map<CategoryViewModel>(targetCategory));
    }

    public async Task<ResponseData> DeleteCategoryByIdAsync(Guid id)
    {
        var target = await _dbContext.Categories.FindAsync(id);
        if (target == null)
        {
            return ResponseData.Error($"category with id {id} is not found.");
        }

        _dbContext.Categories.Remove(target);
        await _dbContext.SaveChangesAsync();

        return ResponseData.Ok();
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


    /// <summary>
    /// 指定された Category の並び順を upCount 個上に移動した時の DisplayOrder を取得します。
    /// </summary>
    /// <param name="target">移動の対象となる Category </param>
    /// <param name="upCount">いくつ上に移動するか(1以上の数値を指定すること)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private async Task<HierarchyId> UpByCount(Category target, int upCount)
    {
        if (upCount <= 0) throw new ArgumentOutOfRangeException(nameof(upCount));

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
        // 移動対象となる category よりも『上に』ある category の一覧を並び順の降順に取得します
        var prevCategories =
            await _dbContext.Categories
                            .Where(cat => cat.DisplayOrder < target.DisplayOrder)
                            .OrderByDescending(cat => cat.DisplayOrder)
                            .ToListAsync();

        if (upCount < prevCategories.Count)
        {
            var overCategory = prevCategories[upCount];
            var underCategory = prevCategories[upCount - 1];
            return HierarchyId.GetRoot()
                              .GetDescendant(overCategory.DisplayOrder, underCategory.DisplayOrder);
        }
        else
        {
            var topCategory = prevCategories.LastOrDefault();
            return HierarchyId.GetRoot().GetDescendant(null, topCategory?.DisplayOrder);
        }
    }

    /// <summary>
    /// 指定された Category の並び順を downCount 個下に移動した時の DisplayOrder を取得します。
    /// </summary>
    /// <param name="target"></param>
    /// <param name="downCount"></param>
    /// <returns></returns>
    private async Task<HierarchyId> DownByCount(Category target, int downCount)
    {
        if (downCount <= 0) throw new ArgumentOutOfRangeException(nameof(downCount));

        // [Old Order]
        //  NameA    /1/   <- target category
        //  NameB    /2/                          0
        //  NameC    /3/   <- 2 down              1
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
                            .Where(cat => cat.DisplayOrder > target.DisplayOrder)
                            .OrderBy(cat => cat.DisplayOrder)
                            .ToListAsync();

        if (downCount < nextCategories.Count)
        {
            var overCategory = nextCategories[downCount - 1];
            var underCategory = nextCategories[downCount];
            return HierarchyId.GetRoot()
                              .GetDescendant(overCategory.DisplayOrder, underCategory.DisplayOrder);
        }
        else
        {
            var bottomCategory = nextCategories.LastOrDefault();
            return HierarchyId.GetRoot().GetDescendant(bottomCategory?.DisplayOrder, null);
        }
    }
}