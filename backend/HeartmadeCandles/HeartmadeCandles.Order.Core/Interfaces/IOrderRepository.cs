using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderRepository
{
    Task<Maybe<Models.Order>> GetOrderById(string orderId);

    Task<(Maybe<Models.Order[]>, long)> GetOrdersWithTotalOrders(OrderFilterParameters queryOptions);

    Task<(Maybe<Models.Order[]>, long)> GetOrderByStatusWithTotalOrders(OrderStatus status, PaginationSettings pagination);

    Task<Result<string>> CreateOrder(Models.Order order);

    Task<Result> UpdateOrder(Models.Order order);

    Task<Result> UpdateOrderStatus(string orderId, OrderStatus status);
}