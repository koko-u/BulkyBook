using BulkyBook.Persistence.Data;
using BulkyBook.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Middleware.Seeder;

public class BulkyBookDbInitializer : IDbInitializer
{
    private readonly BulkyBookDbContext _dbContext;
    private readonly string _art = "Arts & Photography";
    private readonly string _business = "Business & Investing";
    private readonly string _computer = "Computers & Internet";
    private readonly string _health = "Health, Mind & Body";
    private readonly string _fiction = "Literature & Fiction";
    private readonly string _tech = "Professional & Technical";
    private readonly string _religion = "Religion & Spirituality";
    private readonly string _sf = "Science Fiction & Fantasy";


    public BulkyBookDbInitializer(BulkyBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Initialize()
    {
        await _dbContext.Database.EnsureCreatedAsync();
        await InitializeProducts();
    }

    private async Task InitializeCategories()
    {
        if (_dbContext.Categories.Any())
        {
            return;
        }

        var categories = new Category[]
        {
            new() { Name = _art, DisplayOrder = HierarchyId.Parse("/1/") }
            , new() { Name = _business, DisplayOrder = HierarchyId.Parse("/2/") }
            , new() { Name = _computer, DisplayOrder = HierarchyId.Parse("/3/") }
            , new() { Name = _health, DisplayOrder = HierarchyId.Parse("/4/") }
            , new() { Name = _fiction, DisplayOrder = HierarchyId.Parse("/5/") }
            , new() { Name = _tech, DisplayOrder = HierarchyId.Parse("/6/") }
            , new() { Name = _religion, DisplayOrder = HierarchyId.Parse("/7/") }
            , new() { Name = _sf, DisplayOrder = HierarchyId.Parse("/8/") }
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

    private async Task InitializeProducts()
    {
        await InitializeCategories();
        await InitializeCoverTypes();

        if (_dbContext.Products.Any())
        {
            return;
        }

        var art = await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _art));
        var business =
            await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _business));
        var computer =
            await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _computer));
        var health =
            await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _health));
        var fiction =
            await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _fiction));
        var tech =
            await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _tech));
        var religion =
            await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _religion));
        var sf = await _dbContext.Categories.FirstOrDefaultAsync(x => string.Equals(x.Name, _sf));

        var paperback =
            await _dbContext.CoverTypes.FirstOrDefaultAsync(x =>
                string.Equals(x.Name, "Paperback"));
        var hardcover1 =
            await _dbContext.CoverTypes.FirstOrDefaultAsync(x =>
                string.Equals(x.Name, "Hardcover with ImageWrap"));
        var hardcover2 =
            await _dbContext.CoverTypes.FirstOrDefaultAsync(x =>
                string.Equals(x.Name, "Hardcover with DustJacket"));

        var products = new Product[]
        {
            new()
            {
                Title = "Surrender: 40 Songs, One Story"
                , Description =
                    "Bono—artist, activist, and the lead singer of Irish rock band U2—has written a memoir: " +
                    "honest and irreverent, intimate and profound, Surrender is the story of the remarkable life he’s lived, " +
                    "the challenges he’s faced, and the friends and family who have shaped and sustained him."
                , Isbn = "978-0-5255-2104-4"
                , Author = "Bono"
                , ListPrice = 34.00m
                , Price = 22.08m
                , BulkPriceFor50 = 21.00m
                , BulkPriceFor100 = 20.0m
                , ImageUrl =
                    "https://m.media-amazon.com/images/I/41E0-5rnscL._SX336_BO1,204,203,200_.jpg"
                , Category = art ?? new Category()
                , CoverType = paperback ?? new CoverType()
            }
            , new()
            {
                Title = "How To Create Your Own Highly Effective Master Mind Group"
                , Description =
                    "This book explains and gives detailed ways of creating a numerative mastermind group. " +
                    "When you are accountable to someone or a group for doing what you said you would do, " +
                    "you can quickly get stuff done because you engage the power of social expectations. " +
                    "This influence can create a substantial positive impact on your drive and goals."
                , Isbn = "979-8-3614-8219-1"
                , Author = "Abdulrahman, Isa - Abdulrahman, Isa"
                , ListPrice = 10.99m
                , Price = 10.99m
                , BulkPriceFor50 = 10.00m
                , BulkPriceFor100 = 9.98m
                , ImageUrl =
                    "https://m.media-amazon.com/images/I/41UO1FC-1XL._SX322_BO1,204,203,200_.jpg"
                , Category = business ?? new Category()
                , CoverType = paperback ?? new CoverType()
            }
            , new()
            {
                Title = "Chip War: The Fight for the World's Most Critical Technology"
                , Description = "An epic account of the decades-long battle to control " +
                                "what has emerged as the world's most critical resource—microchip technology—with the United States " +
                                "and China increasingly in conflict."
                , Isbn = "978-1-9821-7200-8"
                , Author = "Miller, Chris"
                , ListPrice = 30.0m
                , Price = 24.46m
                , BulkPriceFor50 = 24.0m
                , BulkPriceFor100 = 23.0m
                , ImageUrl =
                    "https://m.media-amazon.com/images/I/41XNS8CsJkL._SX329_BO1,204,203,200_.jpg"
                , Category = computer ?? new Category()
                , CoverType = hardcover1 ?? new CoverType()
            }
            , new()
            {
                Title =
                    "Lighter: Let Go of the Past, Connect with the Present, and Expand the Future"
                , Description =
                    "#1 NEW YORK TIMES BESTSELLER • “An empathetic and wise book that will guide you on a journey " +
                    "toward a deeper understanding of self.”—Nedra Glover Tawwab, LCSW, " +
                    "New York Times bestselling author of Set Boundaries, Find Peace"
                , Isbn = "978-0-5932-3317-7"
                , Author = "Pueblo, Yung"
                , ListPrice = 24.0m
                , Price = 15.88m
                , BulkPriceFor50 = 15.40m
                , BulkPriceFor100 = 15.02m
                , ImageUrl =
                    "https://m.media-amazon.com/images/I/51J9NPOQQwL._SX329_BO1,204,203,200_.jpg"
                , Category = health ?? new Category()
                , CoverType = hardcover2 ?? new CoverType()
            }
        };

        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();
    }
}