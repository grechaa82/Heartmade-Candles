using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IWickRepository
{
    Task<Maybe<Wick[]>> GetAll();
    Task<Maybe<Wick>> Get(int wickId);
    Task<Maybe<Wick[]>> GetByIds(int[] wickIds);
    Task<Result> Create(Wick wick);
    Task<Result> Update(Wick wick);
    Task<Result> Delete(int wickId);
    Task<Result> UpdateCandleWick(int candleId, Wick[] wicks);
}