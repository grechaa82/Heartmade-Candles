namespace HeartmadeCandles.Bot.Core.Models;

public class Basket
{
    public required string Id { get; init; }

    public required BasketItem[] Items { get; init; }

    public decimal TotalPrice { get; init; }

    public int TotalQuantity { get; init; }

    public required string FilterString { get; init; }
}
