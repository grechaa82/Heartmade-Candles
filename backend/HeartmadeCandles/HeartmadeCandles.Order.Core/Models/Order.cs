namespace HeartmadeCandles.Order.Core.Models
{
    public class Order
    {
        public Order(
            string configuredCandlesString, 
            OrderItem[] candles, 
            User user, 
            Feedback feedback)
        {
            ConfiguredCandlesString = configuredCandlesString;
            OrderItems = candles;
            User = user;
            Feedback = feedback;
        }

        public string ConfiguredCandlesString { get; private set; }
        public OrderItem[] OrderItems { get; private set; }
        public User User { get; private set; }
        public Feedback Feedback { get; private set; }

        public decimal GetTotalPrice()
        {
            return OrderItems.Sum(c => c.Price);
        }

        public int GetTotalQuantity()
        {
            return OrderItems.Sum(c => c.Quantity);
        }
    }
}
