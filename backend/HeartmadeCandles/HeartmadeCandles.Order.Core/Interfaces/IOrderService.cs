using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderService
{
    Task<Result<OrderDetail>> GetOrderDetailById(string orderDetailId);

    Task<Result<string>> CreateOrderDetail(OrderDetailItem[] orderItems);

    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<Result<string>> CreateOrder(User user, Feedback feedback, string orderDetailId);
}