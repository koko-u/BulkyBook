using BulkyBook.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Persistence.Data;

public class BulkyBookDbContext : DbContext
{
    public BulkyBookDbContext(DbContextOptions<BulkyBookDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<CoverType> CoverTypes => Set<CoverType>();
}