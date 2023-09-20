using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ICandleRepository
{
    Task<Maybe<Candle[]>> GetAll();
    Task<Maybe<Candle>> GetById(int candleId);
    Task<Maybe<CandleDetail>> GetCandleDetailById(int candleId);
    Task<Result> Create(Candle candle);
    Task<Result> Update(Candle candle);
    Task<Result> Delete(int candleId);
}