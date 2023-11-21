namespace HeartmadeCandles.Order.Core.Models;

public class LayerColor
{
    public LayerColor(
        int id,
        string title,
        decimal pricePerGram)
    {
        Id = id;
        Title = title;
        PricePerGram = pricePerGram;
    }

    public int Id { get; private set; }

    public string Title { get; private set; }

    public decimal PricePerGram { get; }

    public decimal CalculatePriceForGrams(int grams)
    {
        return PricePerGram * grams;
    }
}