using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ITypeCandleService
{
    Task<Maybe<TypeCandle[]>> GetAll();
    Task<Maybe<TypeCandle>> Get(int typeCandleId);
    Task<Result> Create(TypeCandle typeCandle);
    Task<Result> Update(TypeCandle typeCandle);
    Task<Result> Delete(int typeCandleId);
}