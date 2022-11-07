namespace BulkyBook.Presentation.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Isbn { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public decimal ListPrice { get; set; } = decimal.Zero;

    public decimal Price { get; set; } = decimal.Zero;

    public decimal BulkPriceFor50 { get; set; } = decimal.Zero;

    public decimal BulkPriceFor100 { get; set; } = decimal.Zero;

    public string? ImageUrl { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public Guid CoverTypeId { get; set; }

    public string CoverTypeName { get; set; } = string.Empty;
}