using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services;

public class CandleService : ICandleService
{
    private readonly ICandleRepository _candleRepository;
    private readonly IDecorRepository _decorRepository;
    private readonly ILayerColorRepository _layerColorRepository;
    private readonly INumberOfLayerRepository _numberOfLayerRepository;
    private readonly ISmellRepository _smellRepository;
    private readonly ITypeCandleRepository _typeCandleRepository;
    private readonly IWickRepository _wickRepository;

    public CandleService(
        ICandleRepository candleRepository,
        IDecorRepository decorRepository,
        ILayerColorRepository layerColorRepository,
        INumberOfLayerRepository numberOfLayerRepository,
        ISmellRepository smellRepository,
        ITypeCandleRepository typeCandleRepository,
        IWickRepository wickRepository)
    {
        _candleRepository = candleRepository;
        _decorRepository = decorRepository;
        _layerColorRepository = layerColorRepository;
        _numberOfLayerRepository = numberOfLayerRepository;
        _smellRepository = smellRepository;
        _typeCandleRepository = typeCandleRepository;
        _wickRepository = wickRepository;
    }

    public async Task<Result<Candle[]>> GetAll(string? typeFilter, PaginationSettings pagination)
    {
        if (!string.IsNullOrWhiteSpace(typeFilter))
        {
            var typeCandleMaybe = await _typeCandleRepository.Get(typeFilter);

            if (!typeCandleMaybe.HasValue)
            {
                return Result.Failure<Candle[]>($"This type of candle: '{typeFilter}' does not exist");
            }

            var candlesMaybe = await _candleRepository.GetAll(typeCandleMaybe.Value, pagination);
            
            if (!candlesMaybe.HasValue)
            {
                return Result.Failure<Candle[]>("Candles not found");
            }

            return Result.Success(candlesMaybe.Value);
        }
        else
        {
            var candlesMaybe = await _candleRepository.GetAll(null, pagination);

            if (!candlesMaybe.HasValue)
            {
                return Result.Failure<Candle[]>("Candles not found");
            }

            return Result.Success(candlesMaybe.Value);
        }
    }

    public async Task<Maybe<CandleDetail>> Get(int candleId)
    {
        return await _candleRepository.GetCandleDetailById(candleId);
    }

    public async Task<Result> Create(Candle candle)
    {
        return await _candleRepository.Create(candle);
    }

    public async Task<Result> Update(Candle candle)
    {
        return await _candleRepository.Update(candle);
    }

    public async Task<Result> Delete(int candleId)
    {
        return await _candleRepository.Delete(candleId);
    }

    public async Task<Result> UpdateDecor(int candleId, int[] decorIds)
    {
        if (!decorIds.Any())
        {
            return await _decorRepository.UpdateCandleDecor(candleId, Array.Empty<Decor>());
        }

        var decors = await _decorRepository.GetByIds(decorIds);

        if (!decors.HasValue || decors.Value.Length != decorIds.Length)
        {
            var missingIds = decorIds.Except(decors.Value.Select(d => d.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        return await _decorRepository.UpdateCandleDecor(candleId, decors.Value);
    }

    public async Task<Result> UpdateLayerColor(int candleId, int[] layerColorIds)
    {
        if (!layerColorIds.Any())
        {
            return Result.Failure("'LayerColor' cannot be empty");
        }

        var layerColors = await _layerColorRepository.GetByIds(layerColorIds);

        if (!layerColors.HasValue || layerColors.Value.Length != layerColorIds.Length)
        {
            var missingIds = layerColorIds.Except(layerColors.Value.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        return await _layerColorRepository.UpdateCandleLayerColor(candleId, layerColors.Value);
    }

    public async Task<Result> UpdateNumberOfLayer(int candleId, int[] numberOfLayerIds)
    {
        if (!numberOfLayerIds.Any())
        {
            return Result.Failure("'NumberOfLayer' cannot be empty");
        }

        var numberOfLayers = await _numberOfLayerRepository.GetByIds(numberOfLayerIds);

        if (!numberOfLayers.HasValue || numberOfLayers.Value.Length != numberOfLayerIds.Length)
        {
            var missingIds = numberOfLayerIds.Except(numberOfLayers.Value.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        return await _numberOfLayerRepository.UpdateCandleNumberOfLayer(candleId, numberOfLayers.Value);
    }

    public async Task<Result> UpdateSmell(int candleId, int[] smellIds)
    {
        if (!smellIds.Any())
        {
            return await _smellRepository.UpdateCandleSmell(candleId, Array.Empty<Smell>());
        } 

        var smells = await _smellRepository.GetByIds(smellIds);

        if (!smells.HasValue || smells.Value.Length != smellIds.Length)
        {
            var missingIds = smellIds.Except(smells.Value.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        return await _smellRepository.UpdateCandleSmell(candleId, smells.Value);
    }

    public async Task<Result> UpdateWick(int candleId, int[] wickIds)
    {
        if (!wickIds.Any())
        {
            return Result.Failure("'Wick' cannot be empty");
        }

        var wicks = await _wickRepository.GetByIds(wickIds);

        if (!wicks.HasValue || wicks.Value.Length != wickIds.Length)
        {
            var missingIds = wickIds.Except(wicks.Value.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        return await _wickRepository.UpdateCandleWick(candleId, wicks.Value);
    }
}