using BulkyBook.BusinessCore.MappingProfiles;
using BulkyBook.BusinessCore.Services;
using BulkyBook.Configuration;
using BulkyBook.Middleware.Seeder;
using BulkyBook.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Entity framework Core
builder.Services.AddDbContext<BulkyBookDbContext>(opt =>
{
    opt.UseSqlServer(ConnectionStrings.Default, option => option.UseHierarchyId());
});
// Auto Mapper
builder.Services.AddAutoMapper(options => { options.AddProfile<CategoryProfile>(); });

// Services
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

// Middleware
builder.Services.AddScoped<BulkyBookDbInitializer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();