namespace HeartmadeCandles.Constructor.Core.Models;

public class CandleDetail
{
    public Candle Candle { get; init; }
    public Decor[] Decors { get; init; }
    public LayerColor[] LayerColors { get; init; }
    public NumberOfLayer[] NumberOfLayers { get; init; }
    public Smell[] Smells { get; init; }
    public Wick[] Wicks { get; init; }
}