namespace HeartmadeCandles.Constructor.Core.Models
{
    public class Candle
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int WeightGrams { get; init; }
        public Image[] Images { get; init; }
    }
}
