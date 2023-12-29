namespace HeartmadeCandles.Order.Core.Models;

public class Basket
{
    public string? Id { get; init; }

    public required BasketItem[] Items { get; init; }

    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public int TotalQuantity => Items.Sum(x => x.Quantity);

    public required string FilterString { get; init; }
}
