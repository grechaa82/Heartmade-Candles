using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Result<CandleDetailWithQuantityAndPrice[]>> Get(CandleDetailIdsWithQuantity[] arrayCandleDetailIdsWithQuantity);
    }
}
