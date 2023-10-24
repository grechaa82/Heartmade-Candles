using HeartmadeCandles.Order.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Order.DAL.Entities;

[Table("Order")]
public class OrderEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("statusOrderId")]
    public int StatusOrderId { get; set; }

    [Column("createdAt")]
    public int CreatedAt { get; set; }

    [Column("updatedAt")]
    public int UpdatedAt { get; set; }

    [Column("totalPrice")]
    public decimal TotalPrice { get; set; }

    [Column("totalQuantity")]
    public decimal TotalQuantity { get; set; }

    public virtual required ICollection<OrderItemEntity> OrderItem { get; set; }

    public virtual required OrderStatusEntity OrderStatus { get; set; }
}

