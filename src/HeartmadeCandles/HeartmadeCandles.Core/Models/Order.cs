using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class Order : ModelBase
    {
        private User _user;
        private Customer _customer;
        private Dictionary<int, Candle> _candles;
        private Status _status;
        private string _description;
        private DateTime _createdAt;

        [JsonConstructor]
        public Order(
            User user,
            Customer customer,
            Dictionary<int, Candle> candles,
            string id = "",
            Status status = Status.New,
            string description = "")
        {
            if (user == null)
            {
                throw new ArgumentNullException($"'{nameof(user)}' connot be null.");
            }

            if (customer == null)
            {
                throw new ArgumentNullException($"'{nameof(customer)}' connot be null.");
            }

            if (candles == null || !candles.Any())
            {
                throw new ArgumentNullException($"'{nameof(candles)}' connot be null or empty.");
            }

            Id = id;
            _user = user;
            _customer = customer;
            _candles = candles;
            _status = status;
            _description = description;
            _createdAt  = DateTime.UtcNow;
        }

        public User User { get => _user; }
        public Customer Customer { get => _customer; }
        public Dictionary<int, Candle> Candles { get => _candles; }
        public Status Status { get => _status; }
        public string Description { get => _description; }
        public DateTime CreatedAt { get => _createdAt; }

        public Order ChangeStatus(Order order, Status newStatus)
        {
            return new Order(
                order.User,
                order.Customer,
                order.Candles,
                order.Id,
                newStatus,
                order.Description);
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
