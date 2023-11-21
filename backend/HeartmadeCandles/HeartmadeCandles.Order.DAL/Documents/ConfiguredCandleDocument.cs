namespace HeartmadeCandles.Order.DAL.Documents;

public class ConfiguredCandleDocument
{
    public required CandleDocument Candle { get; set; }

    public DecorDocument? Decor { get; set; }

    public required LayerColorDocument[] LayerColors { get; set; }

    public required NumberOfLayerDocument NumberOfLayer { get; set; }

    public SmellDocument? Smell { get; set; }

    public required WickDocument Wick { get; set; }
}

