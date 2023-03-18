namespace HeartmadeCandles.Core.Models
{
    public class Order : ModelBase
    {
        private Order(
            string id,
            User user,
            Customer customer,
            List<Candle> candles,
            Status status,
            string description)
        {
            Id = id;
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

        public static (Order, ErrorDetail[]) Create(
            User user,
            Customer customer,
            List<Candle> candles,
            string description,
            string id = null,
            Status status = Status.New)
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (user == null)
            {
                errorsMessage = $"'{nameof(user)}' connot be null.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (customer == null)
            {
                errorsMessage = $"'{nameof(customer)}' connot be null.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (candles == null || candles.Any())
            {
                errorsMessage = $"'{nameof(candles)}' connot be null or empty.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var order = new Order(
                id,
                user,
                customer,
                candles,
                status,
                description);

            return (order, errors.ToArray());
        }

        public Order ChangeStatus(Order order, Status newStatus)
        {
            return new Order(
                order.Id,
                order.User,
                order.Customer,
                order.Candles,
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
