namespace HeartmadeCandles.Bot.Core.Models;

public class Order
{
    public string Id { get; init; }

    public required Basket Basket { get; init; }

    public Feedback? Feedback { get; init; }

    public OrderStatus Status { get; init; }
}
