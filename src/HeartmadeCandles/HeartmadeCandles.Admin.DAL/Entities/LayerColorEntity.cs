using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("LayerColor")]
    public class LayerColorEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(48), Required]
        public string Title { get; set; }

        [Column("description"), MaxLength(256), Required]
        public string Description { get; set; }

        [Column("pricePerGram")]
        public decimal PricePerGram { get; set; }

        [Column("imageURL")]
        public string ImageURL { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }
    }
}
