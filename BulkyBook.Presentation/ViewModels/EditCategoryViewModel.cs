using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BulkyBook.Persistence.Data;

namespace BulkyBook.Presentation.ViewModels;

public class EditCategoryViewModel : IValidatableObject
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "The Category Name is Required.")]
    [MaxLength(256, ErrorMessage = "Category name is allowed up to 256 chars.")]
    [DisplayName("Category Name")]
    public string? Name { get; set; }

    public string CurrentDisplayOrder { get; set; } = "/";

    [DisplayName("Up or Down")]
    public int UpOrDownCount { get; set; } = 0;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(BulkyBookDbContext)) is not BulkyBookDbContext
            dbContext)
        {
            throw new Exception("BulkyBookDbContext cannot found.");
        }

        if (dbContext.Categories.Any(cat =>
                cat.Id != this.Id && string.Equals(cat.Name, this.Name)))
        {
            yield return new ValidationResult("Category name should be unique.");
        }
    }
}