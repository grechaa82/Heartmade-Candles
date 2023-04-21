using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("TypeCandle")]
    public class TypeCandleEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(32)]
        public string Title { get; set; }

        public ICollection<CandleEntity> Candles { get; set; }
    }
}
