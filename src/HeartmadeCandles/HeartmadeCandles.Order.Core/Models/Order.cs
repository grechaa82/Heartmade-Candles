namespace HeartmadeCandles.Order.Core.Models
{
    public class Order
    {
        public Order(
            string configuredCandlesString, 
            CandleDetailWithQuantityAndPrice[] candles, 
            User user, 
            Feedback feedback)
        {
            ConfiguredCandlesString = configuredCandlesString;
            Candles = candles;
            User = user;
            Feedback = feedback;
        }

        public string ConfiguredCandlesString { get; private set; }
        public CandleDetailWithQuantityAndPrice[] Candles { get; private set; }
        public User User { get; private set; }
        public Feedback Feedback { get; private set; }

        public decimal GetTotalPrice()
        {
            return Candles.Sum(c => c.Price);
        }

        public int GetTotalQuantity()
        {
            return Candles.Sum(c => c.Quantity);
        }
    }
}
