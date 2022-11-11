using System.ComponentModel.DataAnnotations;
using BulkyBook.Validations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Presentation.ViewModels;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Product Title should not be empty.")]
    [StringLength(256, ErrorMessage = "Product Title should be less than 256 characters long.")]
    public string? Title { get; set; }

    [StringLength(4000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Product ISBN code must be specified.")]
    [Isbn]
    public string? Isbn { get; set; }

    [Required(ErrorMessage = "Author name is required.")]
    [StringLength(512)]
    public string? Author { get; set; }

    [Required(ErrorMessage = "List Price is required.")]
    [Range(type: typeof(decimal), minimum: "0.00", maximum: "99999999.99")]
    public decimal? ListPrice { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(type: typeof(decimal), minimum: "0.00", maximum: "99999999.99")]
    public decimal Price { get; set; }

    [Range(type: typeof(decimal), minimum: "0.00", maximum: "99999999.99")]
    public decimal? BulkPriceFor50 { get; set; }

    [Range(type: typeof(decimal), minimum: "0.00", maximum: "99999999.99")]
    public decimal? BulkPriceFor100 { get; set; }

    [Url]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Cover Type is required.")]
    public Guid CoverTypeId { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

    [ValidateNever]
    public IEnumerable<SelectListItem> CoverTypes { get; set; } = new List<SelectListItem>();
}