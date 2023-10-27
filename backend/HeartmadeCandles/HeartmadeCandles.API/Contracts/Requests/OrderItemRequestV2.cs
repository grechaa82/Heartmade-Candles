namespace HeartmadeCandles.API.Contracts.Requests;

public class OrderItemRequestV2
{
    public required CandleRequest Candle { get; set; }
    public DecorRequest? Decor { get; set; }
    public required LayerColorRequest[] LayerColors { get; set; }
    public required NumberOfLayerRequest NumberOfLayer { get; set; }
    public SmellRequest? Smell { get; set; }
    public required WickRequest Wick { get; set; }
    public int Quantity { get; set; }
    public required string ConfigurationString { get; set; }
}

