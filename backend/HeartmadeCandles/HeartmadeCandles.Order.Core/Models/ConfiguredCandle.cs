namespace HeartmadeCandles.Order.Core.Models;

public class ConfiguredCandle
{
    public required Candle Candle { get; init; }
    
    public Decor? Decor { get; init; }
    
    public required LayerColor[] LayerColors { get; init; }
    
    public required NumberOfLayer NumberOfLayer { get; init; }
    
    public Smell? Smell { get; init; }
    
    public required Wick Wick { get; init; }
}
