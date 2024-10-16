using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ITypeCandleRepository
{
    Task<Maybe<TypeCandle[]>> GetAll();

    Task<Maybe<TypeCandle>> Get(int typeCandleId);

    Task<Maybe<TypeCandle>> Get(string typeCandleTitle);

    Task<Result> Create(TypeCandle typeCandle);

    Task<Result> Update(TypeCandle typeCandle);

    Task<Result> Delete(int typeCandleId);
}