namespace HeartmadeCandles.Bot.Core.Models;

public class LayerColor
{
    public int Id { get; init; }

    public required string Title { get; init; }

    public required decimal PricePerGram { get; init; }
}