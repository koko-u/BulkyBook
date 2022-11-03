using BulkyBook.Middleware.Seeder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BulkyBook.Middleware;

public static class UseSeederExtensions
{
    public static IApplicationBuilder UseSeed<TInitializer>(this IApplicationBuilder app)
        where TInitializer : IDbInitializer
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var factory = services.GetRequiredService<TInitializer>();
        factory.Initialize().Wait();

        return app;
    }
}