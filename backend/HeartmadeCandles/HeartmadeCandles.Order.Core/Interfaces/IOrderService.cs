using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderService
{
    Task<Result<Models.Order>> Get(int orderId);

    Task<Result<int>> CreateOrder(OrderItemFilter[] orderItemFilters);

    Task<Result> UpdateOrderStatus(int orderId);

    Task<Result> CheckoutOrder(
        string configuredCandlesString,
        int orderId,
        User user,
        Feedback feedback);
}