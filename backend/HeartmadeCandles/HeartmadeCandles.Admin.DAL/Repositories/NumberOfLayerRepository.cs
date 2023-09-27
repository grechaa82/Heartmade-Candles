using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories;

public class NumberOfLayerRepository : INumberOfLayerRepository
{
    private readonly AdminDbContext _context;

    public NumberOfLayerRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<Maybe<NumberOfLayer[]>> GetAll()
    {
        var items = await _context.NumberOfLayer
            .AsNoTracking()
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<NumberOfLayer[]>.None;
        }

        var result = items
            .Select(NumberOfLayerMapping.MapToNumberOfLayer)
            .ToArray();

        return result;
    }

    public async Task<Maybe<NumberOfLayer>> Get(int numberOfLayerId)
    {
        var item = await _context.NumberOfLayer
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == numberOfLayerId);

        if (item == null)
        {
            return Maybe<NumberOfLayer>.None;
        }

        var numberOfLayer = NumberOfLayerMapping.MapToNumberOfLayer(item);

        return numberOfLayer;
    }

    public async Task<Maybe<NumberOfLayer[]>> GetByIds(int[] numberOfLayerIds)
    {
        var items = await _context.NumberOfLayer
            .AsNoTracking()
            .Where(c => numberOfLayerIds.Contains(c.Id))
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<NumberOfLayer[]>.None;
        }

        var result = items
            .Select(NumberOfLayerMapping.MapToNumberOfLayer)
            .ToArray();

        return result;
    }

    public async Task<Result> Create(NumberOfLayer numberOfLayer)
    {
        var item = NumberOfLayerMapping.MapToNumberOfLayerEntity(numberOfLayer);

        await _context.NumberOfLayer.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure($"NumberOfLayer {numberOfLayer.Number} was not created");
    }

    public async Task<Result> Update(NumberOfLayer numberOfLayer)
    {
        var item = NumberOfLayerMapping.MapToNumberOfLayerEntity(numberOfLayer);

        _context.NumberOfLayer.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"NumberOfLayer {numberOfLayer.Number} was not updated");
    }

    public async Task<Result> Delete(int numberOfLayerId)
    {
        var item = await _context.NumberOfLayer.FirstOrDefaultAsync(c => c.Id == numberOfLayerId);

        if (item == null)
        {
            return Result.Failure($"NumberOfLayer by id: {numberOfLayerId} does not exist");
        }

        _context.NumberOfLayer.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"NumberOfLayer by id: {numberOfLayerId} was not deleted");
    }

    public async Task<Result> UpdateCandleNumberOfLayer(int candleId, NumberOfLayer[] numberOfLayers)
    {
        var existingNumberOfLayers = await _context.CandleNumberOfLayer
            .Where(c => c.CandleId == candleId)
            .ToArrayAsync();

        var numberOfLayersToDelete = existingNumberOfLayers
            .Where(en => numberOfLayers.All(w => w.Id != en.NumberOfLayerId))
            .ToArray();

        var numberOfLayersToAdd = numberOfLayers
            .Where(n => existingNumberOfLayers.All(en => en.NumberOfLayerId != n.Id))
            .Select(n => new CandleEntityNumberOfLayerEntity { CandleId = candleId, NumberOfLayerId = n.Id })
            .ToArray();

        if (!numberOfLayersToDelete.Any() && !numberOfLayersToAdd.Any())
        {
            Result.Failure($"There are no NumberOfLayers of candle by id: {candleId} that need to be updated");
        }

        _context.CandleNumberOfLayer.RemoveRange(numberOfLayersToDelete);
        _context.CandleNumberOfLayer.AddRange(numberOfLayersToAdd);

        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"NumberOfLayers of candle by id: {candleId} were not updated");
    }
}