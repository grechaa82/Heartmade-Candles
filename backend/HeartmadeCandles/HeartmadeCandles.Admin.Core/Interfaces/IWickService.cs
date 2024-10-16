using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IWickService
{
    Task<(Result<Wick[]>, long)> GetAll(PaginationSettings pagination);

    Task<Maybe<Wick>> Get(int wickId);

    Task<Result> Create(Wick wick);

    Task<Result> Update(Wick wick);

    Task<Result> Delete(int wickId);
}