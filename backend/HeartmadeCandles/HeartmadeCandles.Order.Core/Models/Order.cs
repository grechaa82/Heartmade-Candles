using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models;

public class Order
{
    private Order(
        string configuredCandlesString,
        OrderItem[] orderItems,
        User? user,
        Feedback? feedback,
        OrderStatus status)
    {
        ConfiguredCandlesString = configuredCandlesString;
        OrderItems = orderItems;
        User = user;
        Feedback = feedback;
        Status = status;
    }

    public string ConfiguredCandlesString { get; private set; }

    public OrderItem[] OrderItems { get; }

    public User? User { get; private set; }

    public Feedback? Feedback { get; private set; }

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice => OrderItems.Sum(c => c.Price);

    public int TotalQuantity => OrderItems.Sum(c => c.Quantity);

    public static Result<Order> Create(
        string configuredCandlesString,
        OrderItem[] orderItems,
        User? user,
        Feedback? feedback,
        OrderStatus status = OrderStatus.Created)
    {
        var result = Result.Success();

        if (string.IsNullOrWhiteSpace(configuredCandlesString))
        {
            result = Result.Combine(
                result,
                Result.Failure<Order>($"'{nameof(configuredCandlesString)}' cannot be null or whitespace"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<Order>(result.Error);
        }

        var order = new Order(
            configuredCandlesString,
            orderItems,
            user,
            feedback,
            status);

        return Result.Success(order);
    }
}