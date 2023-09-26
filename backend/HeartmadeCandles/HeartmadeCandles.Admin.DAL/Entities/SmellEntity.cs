using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.DAL.Entities;

[Table("Smell")]
public class SmellEntity
{
    [Column("id")] public int Id { get; set; }

    [Column("title")]
    [MaxLength(Smell.MaxTitleLength)]
    [Required]
    public string Title { get; set; }

    [Column("description")]
    [MaxLength(Smell.MaxDescriptionLength)]
    [Required]
    public string Description { get; set; }

    [Column("price")] public decimal Price { get; set; }

    [Column("isActive")] public bool IsActive { get; set; }

    public virtual ICollection<CandleEntitySmellEntity> CandleSmell { get; set; }
}