using BulkyBook.Configuration;
using BulkyBook.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BulkyBook.Persistence.DesignTime;

public class DesignTimeBulkyBookDbContextBuilder : IDesignTimeDbContextFactory<BulkyBookDbContext>
{
    public BulkyBookDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BulkyBookDbContext>()
            .UseSqlServer(ConnectionStrings.Default);

        return new BulkyBookDbContext(builder.Options);
    }
}