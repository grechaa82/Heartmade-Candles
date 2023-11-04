using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.Constructor.Core.Interfaces;

public interface IConstructorService
{
    Task<Result<CandleTypeWithCandles[]>> GetCandles();

    Task<Result<CandleDetail>> GetCandleDetailById(int candleId);

    Task<Result<CandleDetail>> GetCandleByFilter(CandleDetailFilter candleDetailFilter);
}