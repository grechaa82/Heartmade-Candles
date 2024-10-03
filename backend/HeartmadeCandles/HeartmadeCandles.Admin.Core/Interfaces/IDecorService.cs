using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IDecorService
{
    Task<(Result<Decor[]>, long)> GetAll(PaginationSettings pagination);

    Task<Maybe<Decor>> Get(int decorId);

    Task<Result> Create(Decor decor);

    Task<Result> Update(Decor decor);

    Task<Result> Delete(int decorId);
}