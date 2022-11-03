using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Presentation.ViewModels;

public class CategoryViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayOrder { get; set; } = "/";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}