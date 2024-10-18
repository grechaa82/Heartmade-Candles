namespace HeartmadeCandles.Constructor.Core.Models;

public class CandleTypeWithCandles
{
    public required string Type { get; init; }

    public required Candle[] Candles { get; init; }
    
    public long TotalCount { get; init; }
}