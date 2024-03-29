﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities;

[Table("NumberOfLayer")]
public class NumberOfLayerEntity
{
    [Column("id")] public int Id { get; set; }

    [Column("number")] public int Number { get; set; }

    public virtual ICollection<CandleEntityNumberOfLayerEntity> CandleNumberOfLayer { get; set; }
}