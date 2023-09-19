using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Constructor.DAL.Entities;

[Table("LayerColor")]
public class LayerColorEntity
{
    [Column("id")] public int Id { get; set; }

    [Column("title")] public string Title { get; set; }

    [Column("description")] public string Description { get; set; }

    [Column("pricePerGram")] public decimal PricePerGram { get; set; }

    [Column("images", TypeName = "jsonb")] public ImageEntity[] Images { get; set; }

    [Column("isActive")] public bool IsActive { get; set; }

    public virtual ICollection<CandleEntityLayerColorEntity> CandleLayerColor { get; set; }
}