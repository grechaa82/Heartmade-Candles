using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories;

public class LayerColorRepository : ILayerColorRepository
{
    private readonly AdminDbContext _context;

    public LayerColorRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<LayerColor[]> GetAll()
    {
        var items = await _context.LayerColor
            .AsNoTracking()
            .ToArrayAsync();

        var result = items
            .Select(item => LayerColorMapping.MapToLayerColor(item))
            .ToArray();

        return result;
    }

    public async Task<LayerColor> Get(int layerColorId)
    {
        var item = await _context.LayerColor
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == layerColorId);

        var layerColor = LayerColorMapping.MapToLayerColor(item);

        return layerColor;
    }

    public async Task<LayerColor[]> GetByIds(int[] layerColorIds)
    {
        var items = await _context.LayerColor
            .AsNoTracking()
            .Where(c => layerColorIds.Contains(c.Id))
            .ToArrayAsync();

        var result = items
            .Select(item => LayerColorMapping.MapToLayerColor(item))
            .ToArray();

        return result;
    }

    public async Task Create(LayerColor layerColor)
    {
        var item = LayerColorMapping.MapToLayerColorEntity(layerColor);

        await _context.LayerColor.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(LayerColor layerColor)
    {
        var item = LayerColorMapping.MapToLayerColorEntity(layerColor);

        _context.LayerColor.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int layerColorId)
    {
        var item = await _context.LayerColor.FirstOrDefaultAsync(l => l.Id == layerColorId);

        if (item != null)
        {
            _context.LayerColor.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateCandleLayerColor(int candleId, LayerColor[] layerColors)
    {
        var existingLayerColors = await _context.CandleLayerColor
            .Where(c => c.CandleId == candleId)
            .ToArrayAsync();

        var layerColorsToDelete = existingLayerColors
            .Where(el => !layerColors.Any(l => l.Id == el.LayerColorId))
            .ToArray();

        var layerColorssToAdd = layerColors
            .Where(l => !existingLayerColors.Any(el => el.LayerColorId == l.Id))
            .Select(l => new CandleEntityLayerColorEntity { CandleId = candleId, LayerColorId = l.Id })
            .ToArray();

        _context.RemoveRange(layerColorsToDelete);
        _context.AddRange(layerColorssToAdd);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> AreIdsExist(int[] ids)
    {
        foreach (var id in ids)
        {
            var exists = await _context.LayerColor.AnyAsync(l => l.Id == id);

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
            var exists = await _context.LayerColor.AnyAsync(l => l.Id == id);

            if (!exists)
            {
                nonExistingIds.Add(id);
            }
        }

        return nonExistingIds.ToArray();
    }
}