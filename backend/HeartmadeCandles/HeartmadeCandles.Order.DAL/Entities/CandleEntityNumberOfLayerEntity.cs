using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Order.DAL.Entities;

[Table("CandleNumberOfLayer")]
public class CandleEntityNumberOfLayerEntity
{
    [Column("id")] public int Id { get; set; }

    [Column("candleId")]
    [ForeignKey("Candle")]
    public int CandleId { get; set; }

    [Column("numberOfLayerId")]
    [ForeignKey("NumberOfLayer")]
    public int NumberOfLayerId { get; set; }

    public virtual CandleEntity Candle { get; set; }

    public virtual NumberOfLayerEntity NumberOfLayer { get; set; }
}