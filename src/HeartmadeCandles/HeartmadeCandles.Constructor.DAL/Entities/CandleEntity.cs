using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Constructor.DAL.Entities
{
    [Table("Candle")]
    public class CandleEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("weightGrams")]
        public int WeightGrams { get; set; }

        [Column(name: "images", TypeName = "jsonb")]
        public ImageEntity[] Images { get; set; }

        [Column("typeCandleId")]
        public int TypeCandleId { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        public virtual TypeCandleEntity TypeCandle { get; set; }

        public virtual ICollection<CandleEntityDecorEntity> CandleDecor { get; set; }

        public virtual ICollection<CandleEntityLayerColorEntity> CandleLayerColor { get; set; }

        public virtual ICollection<CandleEntityNumberOfLayerEntity> CandleNumberOfLayer { get; set; }

        public virtual ICollection<CandleEntitySmellEntity> CandleSmell { get; set; }

        public virtual ICollection<CandleEntityWickEntity> CandleWick { get; set; }
    }
}