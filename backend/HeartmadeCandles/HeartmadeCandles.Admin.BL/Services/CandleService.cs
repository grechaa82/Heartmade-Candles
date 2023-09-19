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

    public async Task<Candle[]> GetAll()
    {
        return await _candleRepository.GetAll();
    }

    public async Task<CandleDetail> Get(int candleId)
    {
        return await _candleRepository.GetCandleDetailById(candleId);
    }

    public async Task Create(Candle candle)
    {
        await _candleRepository.Create(candle);
    }

    public async Task Update(Candle candle)
    {
        await _candleRepository.Update(candle);
    }

    public async Task Delete(int candleId)
    {
        await _candleRepository.Delete(candleId);
    }

    public async Task<Result> UpdateDecor(int candleId, int[] decorIds)
    {
        var decors = await _decorRepository.GetByIds(decorIds);

        if (decors.Length != decorIds.Length)
        {
            var missingIds = decorIds.Except(decors.Select(d => d.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        await _decorRepository.UpdateCandleDecor(candleId, decors);

        return Result.Success();
    }

    public async Task<Result> UpdateLayerColor(int candleId, int[] layerColorIds)
    {
        var layerColors = await _layerColorRepository.GetByIds(layerColorIds);

        if (layerColors.Length != layerColorIds.Length)
        {
            var missingIds = layerColorIds.Except(layerColors.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        await _layerColorRepository.UpdateCandleLayerColor(candleId, layerColors);

        return Result.Success();
    }

    public async Task<Result> UpdateNumberOfLayer(int candleId, int[] numberOfLayerIds)
    {
        var numberOfLayers = await _numberOfLayerRepository.GetByIds(numberOfLayerIds);

        if (numberOfLayers.Length != numberOfLayerIds.Length)
        {
            var missingIds = numberOfLayerIds.Except(numberOfLayers.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        await _numberOfLayerRepository.UpdateCandleNumberOfLayer(candleId, numberOfLayers);

        return Result.Success();
    }

    public async Task<Result> UpdateSmell(int candleId, int[] smellIds)
    {
        var smells = await _smellRepository.GetByIds(smellIds);

        if (smells.Length != smellIds.Length)
        {
            var missingIds = smellIds.Except(smells.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        await _smellRepository.UpdateCandleSmell(candleId, smells);

        return Result.Success();
    }

    public async Task<Result> UpdateWick(int candleId, int[] wickIds)
    {
        var wicks = await _wickRepository.GetByIds(wickIds);

        if (wicks.Length != wickIds.Length)
        {
            var missingIds = wickIds.Except(wicks.Select(l => l.Id));
            var missingIdsString = string.Join(", ", missingIds);
            return Result.Failure<int[]>($"'{missingIdsString}' these ids do not exist");
        }

        await _wickRepository.UpdateCandleWick(candleId, wicks);

        return Result.Success();
    }
}