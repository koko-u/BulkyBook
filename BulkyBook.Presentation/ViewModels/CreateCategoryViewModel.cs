using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BulkyBook.Persistence.Models;
using BulkyBook.Validations;

namespace BulkyBook.Presentation.ViewModels;

public class CreateCategoryViewModel
{
    [Required(ErrorMessage = "The Category Name is Required.")]
    [StringLength(256, ErrorMessage = "Category name is allowed up to 256 chars.")]
    [IsUnique(ModelType = typeof(Category), ErrorMessage = "The Category Name should be unique.")]
    [DisplayName("Category Name")]
    public string? Name { get; set; }
}