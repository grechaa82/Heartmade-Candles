using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IDecorRepository
{
    Task<Maybe<Decor[]>> GetAll();
    Task<Maybe<Decor>> Get(int decorId);
    Task<Maybe<Decor[]>> GetByIds(int[] decorIds);
    Task<Result> Create(Decor decor);
    Task<Result> Update(Decor decor);
    Task<Result> Delete(int decorId);
    Task<Result> UpdateCandleDecor(int candleId, Decor[] decors);
}