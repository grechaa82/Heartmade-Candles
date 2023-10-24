using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderRepository
{
    Task<Result<OrderItem[]>> Get(int orderId);

    Task<Result<int>> CreateOrder(OrderItemFilter[] orderItemFilters);
}