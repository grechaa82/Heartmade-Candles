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

    public async Task<(Result<Candle[]>, long)> GetCandlesByType(string typeCandle, int pageSize, int pageIndex)
    {
        var typeCandleMaybe = await _constructorRepository.GetTypeCandleByTitle(typeCandle);

        if (!typeCandleMaybe.HasValue) 
        {
            return (Result.Failure<Candle[]>($"This type of candle: '{typeCandle}' does not exist"), 0);
        }

        var (candleMaybe, totalCount) = await _constructorRepository.GetCandlesByType(typeCandleMaybe.Value, pageSize, pageIndex);

        if (!candleMaybe.HasValue)
        {
            return (Result.Failure<Candle[]>("Candles not found"), totalCount);
        }

        return (Result.Success(candleMaybe.Value), totalCount);
    }

    public async Task<Result<CandleDetail>> GetCandleDetailById(int candleId)
    {
        return await _constructorRepository.GetCandleById(candleId)
            .ToResult($"Candle with id '{candleId}' does not exist");
    }

    public async Task<Result<CandleDetail>> GetCandleByFilter(CandleDetailFilter candleDetailFilter)
    {
        return await _constructorRepository.GetCandleByFilter(candleDetailFilter);
    }
}