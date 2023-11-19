using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IBasketService
{
    Task<Result<Basket>> GetBasketById(string basketId);

    Task<Result<string>> CreateBasket(ConfiguredCandleBasket configuredCandleBasket);
}
