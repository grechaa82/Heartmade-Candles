namespace HeartmadeCandles.Order.Core.Models;

public class CandleDetail
{
    public CandleDetail(
        Candle candle,
        Decor? decor,
        LayerColor[] layerColors,
        NumberOfLayer numberOfLayer,
        Smell? smell,
        Wick wick)
    {
        Candle = candle;
        Decor = decor;
        LayerColors = layerColors;
        NumberOfLayer = numberOfLayer;
        Smell = smell;
        Wick = wick;
    }

    public Candle Candle { get; private set; }
    public Decor? Decor { get; private set; }
    public LayerColor[] LayerColors { get; private set; }
    public NumberOfLayer NumberOfLayer { get; private set; }
    public Smell? Smell { get; private set; }
    public Wick Wick { get; private set; }
}