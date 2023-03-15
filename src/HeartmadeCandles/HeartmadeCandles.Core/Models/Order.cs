namespace HeartmadeCandles.Core.Models
{
    public class Order : ModelBase
    {
        public Order(
            User user,
            Customer customer,
            List<Candle> candles,
            string description,
            Status status = Status.New)
        {
            User = user;
            Customer = customer;
            Candles = candles;
            Status = status;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public User User { get; }
        public Customer Customer { get; }
        public List<Candle> Candles { get; }
        public Status Status { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }

        public Order ChangeStatus(Order order, Status newStatus)
        {
            return new Order(
                order.User,
                order.Customer,
                order.Candles,
                order.Description,
                newStatus);
        }
    }
    public enum Status
    {
        New,
        Paid,
        InProgress,
        Completed,
        Shipped,
        Done
    }
}
