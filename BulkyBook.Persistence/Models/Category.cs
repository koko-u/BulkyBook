using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Persistence.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    public HierarchyId DisplayOrder { get; set; } = HierarchyId.GetRoot();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}