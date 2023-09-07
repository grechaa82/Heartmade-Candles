using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.Constructor.Core.Interfaces
{
    public interface IConstructorService
    {
        Task<CandleTypeWithCandles[]> GetCandles();
        Task<Result<CandleDetail>> GetCandleDetailById(int candleId);
    }
}
