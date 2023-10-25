using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderRepository
{
    Task<Result<Models.Order>> GetOrder(int orderId);

    Task<Result<OrderItem[]>> GetOrderItems(OrderItemFilter[] orderItemFilters);

    Task<Result<int>> CreateOrder(Models.Order order);

    Task<Result> UpdateOrder (Models.Order order);
}