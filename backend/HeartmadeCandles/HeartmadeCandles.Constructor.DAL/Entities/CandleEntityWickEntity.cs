using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.Constructor.DAL.Entities
{
    [Table("CandleWick")]
    public class CandleEntityWickEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("candleId"), ForeignKey("Candle")]
        public int CandleId { get; set; }

        [Column("wickId"), ForeignKey("Wick")]
        public int WickId { get; set; }

        public virtual CandleEntity Candle { get; set; }

        public virtual WickEntity Wick { get; set; }
    }
}
