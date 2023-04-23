using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("Wick")]
    public class WickEntity
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
