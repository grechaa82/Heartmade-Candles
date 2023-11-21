namespace HeartmadeCandles.Order.Core.Models;

public class TypeCandle
{
    public TypeCandle(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; private set; }

    public string Title { get; private set; }
}