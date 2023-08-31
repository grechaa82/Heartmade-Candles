using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Order.DAL.Entities
{
    [Table("Wick")]
    public class WickEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column(name: "images", TypeName = "jsonb")]
        public ImageEntity[] Images { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        public virtual ICollection<CandleEntityWickEntity> CandleWick { get; set; }
    }
}
