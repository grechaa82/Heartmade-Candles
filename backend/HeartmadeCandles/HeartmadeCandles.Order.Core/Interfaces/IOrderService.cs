using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderService
{
    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<(Maybe<Models.Order[]>, long)> GetOrdersAndTotalCount(OrderFilterParameters queryOptions);

    Task<(Maybe<Models.Order[]>, long)> GetOrdersByStatusAndTotalCount(OrderStatus status, PaginationSettings pagination);

    Task<Result<string>> CreateOrder(Feedback? feedback, string basketId);

    Task<Result> UpdateOrderStatus(string orderId, OrderStatus status);
}