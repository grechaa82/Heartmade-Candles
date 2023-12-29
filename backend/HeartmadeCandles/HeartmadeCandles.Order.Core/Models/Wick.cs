namespace HeartmadeCandles.Order.Core.Models;

public class Wick
{
    public int Id { get; init; }

    public required string Title { get; init; }

    public required decimal Price { get; init; }
}