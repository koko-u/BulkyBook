using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Persistence.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(256)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string? Description { get; set; }

    [Column(TypeName = "CHAR(20)")]
    public string Isbn { get; set; } = string.Empty;

    [MaxLength(512)]
    public string Author { get; set; } = string.Empty;

    [Precision(10, 2)]
    public decimal ListPrice { get; set; } = decimal.Zero;

    [Precision(10, 2)]
    public decimal Price { get; set; } = decimal.Zero;

    [Precision(10, 2)]
    public decimal BulkPriceFor50 { get; set; } = decimal.Zero;

    [Precision(10, 2)]
    public decimal BulkPriceFor100 { get; set; } = decimal.Zero;

    [MaxLength(2048)]
    public string? ImageUrl { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = new();

    public Guid CoverTypeId { get; set; }

    [ForeignKey(nameof(CoverTypeId))]
    public CoverType CoverType { get; set; } = new();
}