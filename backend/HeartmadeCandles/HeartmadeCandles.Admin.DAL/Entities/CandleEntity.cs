using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.DAL.Entities;

[Table("Candle")]
public class CandleEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [MaxLength(Candle.MaxTitleLength)]
    [Required]
    public string Title { get; set; }

    [Column("description")]
    [MaxLength(Candle.MaxDescriptionLength)]
    [Required]
    public string Description { get; set; }

    [Column("price")] public decimal Price { get; set; }

    [Column("weightGrams")] public int WeightGrams { get; set; }

    [Column("images", TypeName = "jsonb")] public ImageEntity[] Images { get; set; }

    [Column("isActive")] public bool IsActive { get; set; }

    [Column("typeCandleId")] public int TypeCandleId { get; set; }

    [Column("createdAt")] public DateTime CreatedAt { get; set; }

    public virtual TypeCandleEntity TypeCandle { get; set; }

    public virtual ICollection<CandleEntityDecorEntity> CandleDecor { get; set; }

    public virtual ICollection<CandleEntityLayerColorEntity> CandleLayerColor { get; set; }

    public virtual ICollection<CandleEntityNumberOfLayerEntity> CandleNumberOfLayer { get; set; }

    public virtual ICollection<CandleEntitySmellEntity> CandleSmell { get; set; }

    public virtual ICollection<CandleEntityWickEntity> CandleWick { get; set; }
}