using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderService
{
    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<(Maybe<Models.Order[]>, long)> GetOrdersWithTotalOrders(OrderFilterParameters queryOptions);

    Task<(Maybe<Models.Order[]>, long)> GetOrderByStatusWithTotalOrders(OrderStatus status, PaginationSettings pagination);

    Task<Result<string>> CreateOrder(Feedback? feedback, string basketId);

    Task<Result> UpdateOrderStatus(string orderId, OrderStatus status);
}