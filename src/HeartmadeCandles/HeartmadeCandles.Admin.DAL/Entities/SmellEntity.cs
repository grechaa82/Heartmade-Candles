using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("Smell")]
    public class SmellEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(Smell.MaxTitleLenght), Required]
        public string Title { get; set; }

        [Column("description"), MaxLength(Smell.MaxDescriptionLenght), Required]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }
    }
}
