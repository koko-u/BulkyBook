using System.ComponentModel.DataAnnotations;
using BulkyBook.Persistence.Models;
using BulkyBook.Validations;

namespace BulkyBook.Presentation.ViewModels;

public class CreateCategoryViewModel
{
    [Required(ErrorMessage = "The Category Name is Required.")]
    [IsUnique(ModelType = typeof(Category), ErrorMessage = "The Category Name should be unique.")]
    public string? Name { get; set; }
}