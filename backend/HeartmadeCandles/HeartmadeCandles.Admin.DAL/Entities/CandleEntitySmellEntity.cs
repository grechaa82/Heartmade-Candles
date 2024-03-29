﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities;

[Table("CandleSmell")]
public class CandleEntitySmellEntity
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("candleId")]
    [ForeignKey("Candle")]
    public int CandleId { get; set; }

    [Column("smellId")]
    [ForeignKey("Smell")]
    public int SmellId { get; set; }

    public virtual CandleEntity Candle { get; set; }

    public virtual SmellEntity Smell { get; set; }
}