using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("CandleNumberOfLayer")]
    public class CandleEntityNumberOfLayerEntity
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("candleId"), ForeignKey("Candle")]
        public int CandleId { get; set; }

        [Column("numberOfLayerId"), ForeignKey("NumberOfLayer")]
        public int NumberOfLayerId { get; set; }

        public virtual CandleEntity Candle { get; set; }

        public virtual NumberOfLayerEntity NumberOfLayer { get; set; }
    }
}
