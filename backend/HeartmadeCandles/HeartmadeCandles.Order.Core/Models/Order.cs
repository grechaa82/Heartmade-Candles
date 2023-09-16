using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models
{
    public class Order
    {
        private Order(
            string configuredCandlesString, 
            OrderItem[] orderItems, 
            User user, 
            Feedback feedback)
        {
            ConfiguredCandlesString = configuredCandlesString;
            OrderItems = orderItems;
            User = user;
            Feedback = feedback;
        }

        public string ConfiguredCandlesString { get; private set; }
        public OrderItem[] OrderItems { get; private set; }
        public User User { get; private set; }
        public Feedback Feedback { get; private set; }
        public decimal TotalPrice { get => OrderItems.Sum(c => c.Price); }
        public int TotalQuantity { get => OrderItems.Sum(c => c.Quantity); }

        public static Result<Order> Create(
            string configuredCandlesString,
            OrderItem[] orderItems,
            User user,
            Feedback feedback)
        {
            var result = Result.Success();

            if (string.IsNullOrWhiteSpace(configuredCandlesString))
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Order>($"'{nameof(configuredCandlesString)}' cannot be null or whitespace"));
            }

            if (configuredCandlesString.Split(".").Length == orderItems.Length)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Order>($"Length of '{nameof(configuredCandlesString)}' and '{nameof(orderItems)}' does not match"));
            }

            if (result.IsFailure)
            {
                return Result.Failure<Order>(result.Error);
            }

            var order = new Order(
                configuredCandlesString, 
                orderItems, 
                user, 
                feedback);

            return Result.Success(order);
        }
    }
}
