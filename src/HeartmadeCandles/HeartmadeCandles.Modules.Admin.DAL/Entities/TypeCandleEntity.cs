using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Modules.Admin.DAL.Entities
{
    [Table("TypeCandle")]
    public class TypeCandleEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        public ICollection<CandleEntity> Candles { get; set; }
    }
}