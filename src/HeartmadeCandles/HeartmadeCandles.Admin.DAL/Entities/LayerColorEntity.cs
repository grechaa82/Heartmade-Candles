using HeartmadeCandles.Admin.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("LayerColor")]
    public class LayerColorEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(LayerColor.MaxTitleLenght), Required]
        public string Title { get; set; }

        [Column("description"), MaxLength(LayerColor.MaxDescriptionLenght), Required]
        public string Description { get; set; }

        [Column("pricePerGram")]
        public decimal PricePerGram { get; set; }

        [Column("imageURL")]
        public string ImageURL { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        public virtual ICollection<CandleEntityLayerColorEntity> CandleLayerColor { get; set; }
    }
}
