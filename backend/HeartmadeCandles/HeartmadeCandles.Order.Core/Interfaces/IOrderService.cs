using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IOrderService
{
    Task<Result<Basket>> GetBasketById(string orderDetailId);

    Task<Result<string>> CreateBasket(ConfiguredCandleFilter[] candleDetailsFilters);

    Task<Result<Models.Order>> GetOrderById(string orderId);

    Task<Result<string>> CreateOrder(User user, Feedback feedback, string basketId);
}