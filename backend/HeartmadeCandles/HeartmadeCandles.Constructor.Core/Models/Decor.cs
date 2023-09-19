namespace HeartmadeCandles.Constructor.Core.Models;

public class Decor
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public Image[] Images { get; init; }
}