namespace HeartmadeCandles.Order.Core.Models;

public class LayerColor
{
    public int Id { get; init; }

    public required string Title { get; init; }

    public required decimal PricePerGram { get; init; }

    public decimal CalculatePriceForGrams(int grams)
    {
        return PricePerGram * grams;
    }
}