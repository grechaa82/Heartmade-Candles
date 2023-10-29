namespace HeartmadeCandles.Order.DAL.Mongo.Collections;

public class OrderDetailItemCollection
{
    public required CandleCollection Candle { get; set; }

    public DecorCollection? Decor { get; set; }

    public required LayerColorCollection[] LayerColors { get; set; }

    public required NumberOfLayerCollection NumberOfLayer { get; set; }

    public SmellCollection? Smell { get; set; }

    public required WickCollection Wick { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public required string ConfigurationString { get; set; }
}

