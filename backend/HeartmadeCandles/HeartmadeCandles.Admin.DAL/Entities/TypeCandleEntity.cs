using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.DAL.Entities;

[Table("TypeCandle")]
public class TypeCandleEntity
{
    [Column("id")] public int Id { get; set; }

    [Column("title")]
    [MaxLength(TypeCandle.MaxTitleLength)]
    [Required]
    public string Title { get; set; }

    public ICollection<CandleEntity> Candles { get; set; }
}