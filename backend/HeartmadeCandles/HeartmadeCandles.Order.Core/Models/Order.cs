namespace HeartmadeCandles.Order.Core.Models;

public class Order
{
    public string Id { get; set; }

    public required string OrderDetailId { get; set; }

    public OrderDetail? OrderDetail { get; set; }
    
    public required User User { get; set; }

    public required Feedback Feedback { get; set; }

    public OrderStatus Status { get; set; }
}

