namespace HeartmadeCandles.Order.DAL.Mongo.Collections;

public class LayerColorCollection
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal PricePerGram { get; set; }

    public ImageCollection[] Images { get; set; }
}