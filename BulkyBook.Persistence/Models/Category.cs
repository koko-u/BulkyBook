using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Persistence.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    public uint DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}