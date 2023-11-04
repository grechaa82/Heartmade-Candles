using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderRepository
{
    Task<Result<Basket>> GetBasketById(string basketId);

    Task<Result<string>> CreateBasket(Basket basket);

    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<Result<string>> CreateOrder(Models.Order order);
}