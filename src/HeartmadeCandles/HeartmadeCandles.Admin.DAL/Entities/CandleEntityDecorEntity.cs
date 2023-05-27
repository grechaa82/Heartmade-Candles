using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("CandleDecor")]
    public class CandleEntityDecorEntity
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("candleId")]
        public int CandleId { get; set; }

        [Column("decorId")]
        public int DecorId { get; set; }

        public virtual CandleEntity Candle { get; set; }

        public virtual DecorEntity Decor { get; set; }
    }
}
