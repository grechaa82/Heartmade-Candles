namespace HeartmadeCandles.Constructor.Core.Models
{
    public class Candle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int WeightGrams { get; set; }
        public string ImageURL { get; set; }
    }
}
