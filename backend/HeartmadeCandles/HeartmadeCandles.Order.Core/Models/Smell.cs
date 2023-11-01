namespace HeartmadeCandles.Order.Core.Models;

public class Smell
{
    public Smell(
        int id,
        string title,
        decimal price)
    {
        Id = id;
        Title = title;
        Price = price;
    }

    public int Id { get; private set; }

    public string Title { get; private set; }

    public decimal Price { get; private set; }
}