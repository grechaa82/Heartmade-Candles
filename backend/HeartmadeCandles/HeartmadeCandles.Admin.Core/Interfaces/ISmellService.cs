using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ISmellService
{
    Task<(Result<Smell[]>, long)> GetAll(PaginationSettings pagination);

    Task<Maybe<Smell>> Get(int smellId);

    Task<Result> Create(Smell smell);

    Task<Result> Update(Smell smell);

    Task<Result> Delete(int smellId);
}