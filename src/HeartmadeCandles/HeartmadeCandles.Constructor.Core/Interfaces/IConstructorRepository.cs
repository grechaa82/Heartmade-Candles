using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.Constructor.Core.Interfaces
{
    public interface IConstructorRepository
    {
        Task<Maybe<CandleDetail>> GetCandleById(int candleId);
        Task<CandleTypeWithCandles[]> GetCandles();
    }
}
