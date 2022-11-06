using BulkyBook.Persistence.Data;
using BulkyBook.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Middleware.Seeder;

public class BulkyBookDbInitializer : IDbInitializer
{
    private readonly BulkyBookDbContext _dbContext;

    public BulkyBookDbInitializer(BulkyBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Initialize()
    {
        await _dbContext.Database.EnsureCreatedAsync();
        await InitializeCategories();
        await InitializeCoverTypes();
    }

    private async Task InitializeCategories()
    {
        if (_dbContext.Categories.Any())
        {
            return;
        }

        var categories = new Category[]
        {
            new() { Name = "Arts & Photography", DisplayOrder = HierarchyId.Parse("/1/") }
            , new() { Name = "Business & Investing", DisplayOrder = HierarchyId.Parse("/2/") }
            , new() { Name = "Computers & Internet", DisplayOrder = HierarchyId.Parse("/3/") }
            , new() { Name = "Health, Mind & Body", DisplayOrder = HierarchyId.Parse("/4/") }
            , new() { Name = "Literature & Fiction", DisplayOrder = HierarchyId.Parse("/5/") }
            , new() { Name = "Professional & Technical", DisplayOrder = HierarchyId.Parse("/6/") }
            , new() { Name = "Religion & Spirituality", DisplayOrder = HierarchyId.Parse("/7/") }
            , new() { Name = "Science Fiction & Fantasy", DisplayOrder = HierarchyId.Parse("/8/") }
        };

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();
    }

    private async Task InitializeCoverTypes()
    {
        if (_dbContext.CoverTypes.Any())
        {
            return;
        }

        var coverTypes = new CoverType[]
        {
            new() { Name = "Paperback" }
            , new() { Name = "Hardcover with ImageWrap" }
            , new() { Name = "Hardcover with DustJacket" }
        };

        await _dbContext.CoverTypes.AddRangeAsync(coverTypes);
        await _dbContext.SaveChangesAsync();
    }
}