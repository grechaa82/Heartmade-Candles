using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("Smell")]
    public class SmellEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(48), Required]
        public string Title { get; set; }

        [Column("description"), MaxLength(256), Required]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }
    }
}
