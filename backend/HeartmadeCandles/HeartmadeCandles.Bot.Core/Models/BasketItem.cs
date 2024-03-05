namespace HeartmadeCandles.Bot.Core.Models;

public class BasketItem
{
    public required ConfiguredCandle ConfiguredCandle { get; init; }

    public decimal Price { get; init; }

    public int Quantity { get; init; }

    public string? ConfiguredCandleFilterString { get; init; }
}
