using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.Core.Models;

namespace HeartmadeCandles.Constructor.BL.Services;

public class ConstructorService : IConstructorService
{
    private readonly IConstructorRepository _constructorRepository;

    public ConstructorService(IConstructorRepository constructorRepository)
    {
        _constructorRepository = constructorRepository;
    }

    public async Task<Result<CandleTypeWithCandles[]>> GetCandles()
    {
        return await _constructorRepository.GetCandles();
    }

    public async Task<Result<CandleDetail>> GetCandleDetailById(int candleId)
    {
        return await _constructorRepository.GetCandleById(candleId)
            .ToResult($"Candle with id '{candleId}' does not exist");
    }

    public Task<Result<CandleDetail>> GetCandleByFilter(int candleId, int? decorId, int numberOfLayerId, int[] layerColorIds, int? smellId, int wickId)
    {
        // return await _constructorRepository.GetCandleByFilter(int candleId, int decorId, int numberOfLayerId, int[] layerColorIds, int smellId, int wickId);
        throw new NotImplementedException();
    }
}