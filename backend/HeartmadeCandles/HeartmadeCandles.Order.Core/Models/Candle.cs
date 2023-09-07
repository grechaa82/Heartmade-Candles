namespace HeartmadeCandles.Order.Core.Models
{
    public class Candle
    {
        public Candle(
            int id, 
            string title, 
            string description, 
            decimal price, 
            int weightGrams, 
            Image[] images)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            WeightGrams = weightGrams;
            Images = images;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int WeightGrams { get; private set; }
        public Image[] Images { get; private set; }
    }
}
