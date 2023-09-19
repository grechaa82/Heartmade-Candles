namespace HeartmadeCandles.Order.Core.Models;

public class Wick
{
    public Wick(
        int id,
        string title,
        string description,
        decimal price,
        Image[] images)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        Images = images;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public Image[] Images { get; private set; }
}