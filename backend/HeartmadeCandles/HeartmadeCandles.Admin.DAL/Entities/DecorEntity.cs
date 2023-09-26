using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.DAL.Entities;

[Table("Decor")]
public class DecorEntity
{
    [Column("id")] public int Id { get; set; }

    [Column("title")]
    [MaxLength(Decor.MaxTitleLength)]
    [Required]
    public string Title { get; set; }

    [Column("description")]
    [MaxLength(Decor.MaxDescriptionLength)]
    [Required]
    public string Description { get; set; }

    [Column("price")] public decimal Price { get; set; }

    [Column("images", TypeName = "jsonb")] public ImageEntity[] Images { get; set; }

    [Column("isActive")] public bool IsActive { get; set; }

    public virtual ICollection<CandleEntityDecorEntity> CandleDecor { get; set; }
}