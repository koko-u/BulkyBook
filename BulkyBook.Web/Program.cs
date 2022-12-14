using BulkyBook.BusinessCore.MappingProfiles;
using BulkyBook.BusinessCore.Services;
using BulkyBook.Configuration;
using BulkyBook.Middleware;
using BulkyBook.Middleware.Seeder;
using BulkyBook.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Entity framework Core
builder.Services.AddDbContext<BulkyBookDbContext>(options =>
{
    options.UseSqlServer(
        ConnectionStrings.Default
        , opt => opt.UseHierarchyId());
});

// Auto Mapper
builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<CategoryProfile>();
    options.AddProfile<CoverTypeProfile>();
});

// Services
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<ICoverTypesService, CoverTypesService>();

// Middleware
builder.Services.AddScoped<BulkyBookDbInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSeed<BulkyBookDbInitializer>();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();