using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Order.DAL.Entities;

[Table("TypeCandle")]
public class TypeCandleEntity
{
    [Column("id")] public int Id { get; set; }

    [Column("title")] [Required] public string Title { get; set; }

    public ICollection<CandleEntity> Candles { get; set; }
}