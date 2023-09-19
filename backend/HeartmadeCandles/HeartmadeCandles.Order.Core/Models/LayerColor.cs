namespace HeartmadeCandles.Order.Core.Models;

public class LayerColor
{
    public LayerColor(
        int id,
        string title,
        string description,
        decimal pricePerGram,
        Image[] images)
    {
        Id = id;
        Title = title;
        Description = description;
        PricePerGram = pricePerGram;
        Images = images;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal PricePerGram { get; private set; }
    public Image[] Images { get; private set; }
}