using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Order.DAL.Entities;

public class OrderStatusEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("status")]
    public required string Status { get; set; }
}
