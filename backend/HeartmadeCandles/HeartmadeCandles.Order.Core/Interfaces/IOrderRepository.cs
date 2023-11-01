using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderRepository
{
    Task<Result<OrderDetail>> GetOrderDetailById(string id);

    Task<Result<string>> CreateOrderDetail(OrderDetail orderDetail);

    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<Result<string>> CreateOrder(Models.Order order);
}