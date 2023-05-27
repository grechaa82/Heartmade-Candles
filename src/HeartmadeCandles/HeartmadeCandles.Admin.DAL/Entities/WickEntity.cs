using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("Wick")]
    public class WickEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(Wick.MaxTitleLenght), Required]
        public string Title { get; set; }

        [Column("description"), MaxLength(Wick.MaxDescriptionLenght), Required]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("imageURL")]
        public string ImageURL { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        public virtual ICollection<CandleEntityWickEntity> CandleWick { get; set; }
    }
}
