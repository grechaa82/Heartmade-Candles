namespace HeartmadeCandles.Order.Core.Models;

public class ConfiguredCandle
{
    public required Candle Candle { get; set; }
    
    public Decor? Decor { get; set; }
    
    public required LayerColor[] LayerColors { get; set; }
    
    public required NumberOfLayer NumberOfLayer { get; set; }
    
    public Smell? Smell { get; set; }
    
    public required Wick Wick { get; set; }
}
