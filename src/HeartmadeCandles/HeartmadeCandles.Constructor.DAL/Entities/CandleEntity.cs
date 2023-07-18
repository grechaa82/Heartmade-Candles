using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.Constructor.DAL.Entities
{
    [Table("Candle")]
    public class CandleEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
        public int Id { get; set; }

        [Column("title"), Required]
        public string Title { get; set; }

        [Column("description"), Required]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("weightGrams")]
        public int WeightGrams { get; set; }

        [Column("imageURL")]
        public string ImageURL { get; set; }

        [Column("typeCandleId")]
        public int TypeCandleId { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        public virtual TypeCandleEntity TypeCandle { get; set; }
    }
}