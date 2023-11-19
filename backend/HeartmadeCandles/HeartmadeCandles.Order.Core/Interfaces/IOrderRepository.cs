using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderRepository
{
    Task<Maybe<Models.Order>> GetOrderById(string orderId);

    Task<Result<string>> CreateOrder(Models.Order order);
}