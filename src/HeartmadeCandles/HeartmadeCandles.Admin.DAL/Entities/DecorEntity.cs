using HeartmadeCandles.Admin.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("Decor")]
    public class DecorEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(Decor.MaxTitleLenght), Required] 
        public string Title { get; set; }

        [Column("description"), MaxLength(Decor.MaxDescriptionLenght), Required]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("imageURL")]
        public string ImageURL { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        public virtual ICollection<CandleEntityDecorEntity> CandleDecor { get; set; }
    }
}
