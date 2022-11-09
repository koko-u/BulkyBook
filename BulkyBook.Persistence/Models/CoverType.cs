using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Persistence.Models;

[Index(nameof(Name), IsUnique = true)]
public class CoverType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
}