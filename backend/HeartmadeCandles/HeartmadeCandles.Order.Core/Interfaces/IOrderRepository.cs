using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderRepository
{
    Task<Maybe<Models.Order>> GetOrderById(string orderId);

    Task<Maybe<Models.Order[]>> GetOrderByStatus(OrderStatus status);

    Task<Result<string>> CreateOrder(Models.Order order);

    Task<Result> UpdateOrderStatus(string orderId, OrderStatus status);
}