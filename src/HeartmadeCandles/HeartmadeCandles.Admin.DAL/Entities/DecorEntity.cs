using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("Decor")]
    public class DecorEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(48)] 
        public string Title { get; set; }

        [Column("description"), MaxLength(256)]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("imageURL")]
        public string ImageURL { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }
    }
}
