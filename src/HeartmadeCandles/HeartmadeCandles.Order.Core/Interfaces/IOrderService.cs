using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Result<CandleDetailWithQuantityAndPrice[]>> Get(CandleDetailIdsWithQuantity[] arrayCandleDetailIdsWithQuantity);
        Task<Result> CreateOrder(string configuredCandlesString, CandleDetailIdsWithQuantity[] arrayCandleDetailIdsWithQuantity, User user, Feedback feedback);
    }
}
