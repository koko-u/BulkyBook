using System.ComponentModel.DataAnnotations;
using BulkyBook.Persistence.Data;

namespace BulkyBook.Presentation.ViewModels;

public class EditCoverTypeViewModel : IValidatableObject
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cover Type Name must be specified")]
    [MaxLength(256, ErrorMessage = "Cover Type Name is up to 256 characters")]
    public string? Name { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.GetService(typeof(BulkyBookDbContext)) is not BulkyBookDbContext
            dbContext)
        {
            throw new Exception("BulkyBookDbContext cannot found.");
        }

        if (dbContext.CoverTypes.Any(ct => ct.Id != this.Id && string.Equals(ct.Name, this.Name)))
        {
            yield return new ValidationResult("Cover Type name should be unique."
                , new[] { nameof(Name) });
        }
    }
}