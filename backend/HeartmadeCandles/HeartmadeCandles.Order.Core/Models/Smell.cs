namespace HeartmadeCandles.Order.Core.Models
{
    public class Smell
    {
        public Smell(
            int id, 
            string title, 
            string description, 
            decimal price)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
    }
}