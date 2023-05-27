using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("CandleLayerColor")]
    public class CandleEntityLayerColorEntity
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("candleId"), ForeignKey("Candle")]
        public int CandleId { get; set; }

        [Column("layerColorId"), ForeignKey("LayerColor")]
        public int LayerColorId { get; set; }

        public virtual CandleEntity Candle { get; set; }

        public virtual LayerColorEntity LayerColor { get; set; }
    }
}
