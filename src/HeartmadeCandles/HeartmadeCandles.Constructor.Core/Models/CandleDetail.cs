namespace HeartmadeCandles.Constructor.Core.Models
{
    public class CandleDetail
    {
        public Candle Candle { get; set; }
        public Decor[] Decors { get; set; }
        public LayerColor[] LayerColors { get; set; }
        public NumberOfLayer[] NumberOfLayers { get; set; }
        public Smell[] Smells { get; set; }
        public Wick[] Wicks { get; set; }
    }
}
