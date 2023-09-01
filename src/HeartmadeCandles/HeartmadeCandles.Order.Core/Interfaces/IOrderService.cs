using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Result<CandleDetailWithQuantityAndPrice[]>> Get(CandleDetailIdsWithQuantity[] arrayCandleDetailIdsWithQuantity);
        Task<Result> CreateOrder(CandleDetailIdsWithQuantity[] arrayCandleDetailIdsWithQuantity, User user, Feedback feedback);
    }
}
