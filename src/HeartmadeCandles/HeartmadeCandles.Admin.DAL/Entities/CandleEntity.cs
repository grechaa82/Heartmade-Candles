﻿using HeartmadeCandles.Admin.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    [Table("Candle")]
    public class CandleEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title"), MaxLength(Candle.MaxTitleLenght), Required] 
        public string Title { get; set; }

        [Column("description"), MaxLength(Candle.MaxDescriptionLenght), Required]
        public string Description { get; set; }

        [Column("weightGrams")]
        public int WeightGrams { get; set; }
        
        [Column("imageURL")]
        public string ImageURL { get; set; }

        [Column("isActive")]
        public bool IsActive { get; set; }

        [Column("typeCandleId")]
        public int TypeCandleId { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        public virtual TypeCandleEntity TypeCandle { get; set; }
    }
}
