using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.Constructor.DAL.Entities
{
    [Table("TypeCandle")]
    public class TypeCandleEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), Required]
        public string Title { get; set; }

        public ICollection<CandleEntity> Candles { get; set; }
    }
}