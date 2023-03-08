namespace HeartmadeCandles.Core.Models
{
    public class Order : ModelBase
    {
        public Order(
    User user,
    Customer customer,
    List<Candle> candles,
    string description)
        {
            User = user;
            Customer = customer;
            Candles = candles;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
        public User User { get; }
        public Customer Customer { get; }
        public List<Candle> Candles { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
    }
}
