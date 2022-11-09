using System.ComponentModel.DataAnnotations;
using BulkyBook.Persistence.Models;
using BulkyBook.Validations;

namespace BulkyBook.Presentation.ViewModels;

public class CreateCoverTypeViewModel
{
    [Required(ErrorMessage = "Cover Type Name must be specified")]
    [StringLength(256, ErrorMessage = "Cover Type Name is up to 256 characters")]
    [IsUnique(ModelType = typeof(CoverType)
        , ErrorMessage = "Cover Type Name should have unique name")]
    public string? Name { get; set; }
}