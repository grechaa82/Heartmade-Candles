using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services;

public class DecorService : IDecorService
{
    private readonly IDecorRepository _decorRepository;

    public DecorService(IDecorRepository decorRepository)
    {
        _decorRepository = decorRepository;
    }

    public async Task<(Result<Decor[]>, long)> GetAll(PaginationSettings pagination)
    {
        var (decorsMaybe, totalCount) = await _decorRepository.GetAll(pagination);

        if (!decorsMaybe.HasValue)
        {
            return (Result.Failure<Decor[]>("Decors not found"), totalCount);
        }

        return (Result.Success(decorsMaybe.Value), totalCount);
    }

    public async Task<Maybe<Decor>> Get(int decorId)
    {
        return await _decorRepository.Get(decorId);
    }

    public async Task<Result> Create(Decor decor)
    {
        return await _decorRepository.Create(decor);
    }

    public async Task<Result> Update(Decor decor)
    {
        return await _decorRepository.Update(decor);
    }

    public async Task<Result> Delete(int decorId)
    {
        return await _decorRepository.Delete(decorId);
    }
}