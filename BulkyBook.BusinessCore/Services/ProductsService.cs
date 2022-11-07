using AutoMapper;
using BulkyBook.Persistence.Data;
using BulkyBook.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BulkyBook.BusinessCore.Services;

public class ProductsService : IProductsService
{
    private readonly BulkyBookDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(BulkyBookDbContext dbContext
        , IMapper mapper
        , ILogger<ProductsService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
    {
        var products = await _dbContext.Products.ToListAsync();
        return _mapper.Map<IEnumerable<ProductViewModel>>(products);
    }
}