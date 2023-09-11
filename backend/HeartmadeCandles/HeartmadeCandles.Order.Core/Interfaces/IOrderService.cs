using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderItem[]>> Get(OrderItemFilter[] OrderItemFilters);
        Task<Result> CreateOrder(string configuredCandlesString, OrderItemFilter[] OrderItemFilters, User user, Feedback feedback);
    }
}
