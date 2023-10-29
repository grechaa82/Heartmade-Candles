using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IMongoRepository
{
    Task<Result<string>> CreateOrderDetail(OrderDetail orderDetail);

    Task<Result<OrderDetail>> GetOrderDetailById(string id);

    Task<Result> CreateOrder(OrderV2  order);
}

