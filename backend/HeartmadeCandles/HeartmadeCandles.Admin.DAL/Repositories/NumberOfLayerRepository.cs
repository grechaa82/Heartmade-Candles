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

    public async Task<NumberOfLayer[]> GetAll()
    {
        var items = await _context.NumberOfLayer
            .AsNoTracking()
            .ToArrayAsync();

        var result = items
            .Select(item => NumberOfLayerMapping.MapToNumberOfLayer(item))
            .ToArray();

        return result;
    }

    public async Task<NumberOfLayer> Get(int numberOfLayerId)
    {
        var item = await _context.NumberOfLayer
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == numberOfLayerId);

        var numberOfLayer = NumberOfLayerMapping.MapToNumberOfLayer(item);

        return numberOfLayer;
    }

    public async Task<NumberOfLayer[]> GetByIds(int[] numberOfLayerIds)
    {
        var items = await _context.NumberOfLayer
            .AsNoTracking()
            .Where(c => numberOfLayerIds.Contains(c.Id))
            .ToArrayAsync();

        var result = items
            .Select(item => NumberOfLayerMapping.MapToNumberOfLayer(item))
            .ToArray();

        return result;
    }

    public async Task Create(NumberOfLayer numberOfLayer)
    {
        var item = NumberOfLayerMapping.MapToNumberOfLayerEntity(numberOfLayer);

        await _context.NumberOfLayer.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(NumberOfLayer numberOfLayer)
    {
        var item = NumberOfLayerMapping.MapToNumberOfLayerEntity(numberOfLayer);

        _context.NumberOfLayer.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int numberOfLayerId)
    {
        var item = await _context.NumberOfLayer.FirstOrDefaultAsync(c => c.Id == numberOfLayerId);

        if (item != null)
        {
            _context.NumberOfLayer.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateCandleNumberOfLayer(int candleId, NumberOfLayer[] numberOfLayers)
    {
        var existingNumberOfLayers = await _context.CandleNumberOfLayer
            .Where(c => c.CandleId == candleId)
            .ToArrayAsync();

        var numberOfLayersToDelete = existingNumberOfLayers
            .Where(en => !numberOfLayers.Any(w => w.Id == en.NumberOfLayerId))
            .ToArray();

        var numberOfLayersToAdd = numberOfLayers
            .Where(n => !existingNumberOfLayers.Any(en => en.NumberOfLayerId == n.Id))
            .Select(n => new CandleEntityNumberOfLayerEntity { CandleId = candleId, NumberOfLayerId = n.Id })
            .ToArray();

        _context.RemoveRange(numberOfLayersToDelete);
        _context.AddRange(numberOfLayersToAdd);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> AreIdsExist(int[] ids)
    {
        foreach (var id in ids)
        {
            var exists = await _context.Decor.AnyAsync(d => d.Id == id);

            if (!exists)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<int[]> GetNonExistingIds(int[] ids)
    {
        var nonExistingIds = new List<int>();

        foreach (var id in ids)
        {
            var exists = await _context.Decor.AnyAsync(d => d.Id == id);

            if (!exists)
            {
                nonExistingIds.Add(id);
            }
        }

        return nonExistingIds.ToArray();
    }
}