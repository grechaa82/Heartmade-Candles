namespace HeartmadeCandles.Order.Core.Models;

public class Candle
{
    public int Id { get; init; }

    public required string Title { get; init; }

    public decimal Price { get; init; }

    public int WeightGrams { get; init; }

    public required Image[] Images { get; init; }
}