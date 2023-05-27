using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("CandleWick")]
    public class CandleEntityWickEntity
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("candleId"), ForeignKey("Candle")]
        public int CandleId { get; set; }

        [Column("wickId"), ForeignKey("Wick")]
        public int WickId { get; set; }

        public virtual CandleEntity Candle { get; set; }

        public virtual WickEntity Wick { get; set; }
    }
}
