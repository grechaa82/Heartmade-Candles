using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Order.DAL.Entities;

public class OrderItemEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("candleId")]
    public int CandleId { get; set; }

    [Column("decorId")]
    public int? DecorId { get; set; }

    [Column("layerColorIds", TypeName = "jsonb")]
    public required int[] LayerColorIds { get; set; }

    [Column("numberOfLayerId")]
    public int NumberOfLayerId { get; set; }

    [Column("smellId")]
    public int? SmellId { get; set; }

    [Column("wickId")]
    public int WickId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    public virtual required CandleEntity Candle { get; set; }

    public virtual DecorEntity? Decor { get; set; }

    public virtual required ICollection<LayerColorEntity[]> LayerColors { get; set; }

    public virtual required NumberOfLayerEntity NumberOfLayer { get; set; }

    public virtual SmellEntity? Smell { get; set; }

    public virtual required WickEntity Wick { get; set; }
}

