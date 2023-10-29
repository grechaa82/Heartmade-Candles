namespace HeartmadeCandles.Order.DAL.Mongo.Collections;


public class CandleCollection
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int WeightGrams { get; set; }

    public ImageCollection[] Images { get; set; }
}