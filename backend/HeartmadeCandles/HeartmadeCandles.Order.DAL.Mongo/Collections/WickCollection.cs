namespace HeartmadeCandles.Order.DAL.Mongo.Collections;

public class WickCollection
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public ImageCollection[] Images { get; set; }
}