namespace HeartmadeCandles.Order.Core.Models;

public class Basket
{
    public string? Id { get; set; }

    public required BasketItem[] Items { get; set; }

    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public int TotalQuantity => Items.Sum(x => x.Quantity);

    public required string FilterString { get; set; }
}
