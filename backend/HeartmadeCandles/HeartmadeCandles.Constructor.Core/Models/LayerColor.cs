namespace HeartmadeCandles.Constructor.Core.Models;

public class LayerColor
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public decimal PricePerGram { get; init; }
    public Image[] Images { get; init; }
}