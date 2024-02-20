using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderService
{
    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<Maybe<Models.Order[]>> GetOrderByStatus(OrderStatus status);

    Task<Maybe<Models.Order[]>> GetOrderByStatus(OrderStatus status, int pageSige, int pageIndex);

    Task<Result<string>> CreateOrder(Feedback? feedback, string basketId);

    Task<Result> UpdateOrderStatus(string orderId, OrderStatus status);
}