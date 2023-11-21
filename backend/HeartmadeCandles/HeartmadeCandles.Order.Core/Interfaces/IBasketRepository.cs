using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces;

public interface IBasketRepository
{
    Task<Maybe<Basket>> GetBasketById(string basketId);

    Task<Result<string>> CreateBasket(Basket basket);    
}
