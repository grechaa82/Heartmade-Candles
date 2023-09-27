using CSharpFunctionalExtensions;
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

    public async Task<Maybe<LayerColor[]>> GetAll()
    {
        var items = await _context.LayerColor
            .AsNoTracking()
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<LayerColor[]>.None;
        }

        var result = items
            .Select(LayerColorMapping.MapToLayerColor)
            .ToArray();

        return result;
    }

    public async Task<Maybe<LayerColor>> Get(int layerColorId)
    {
        var item = await _context.LayerColor
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == layerColorId);

        if (item == null)
        {
            return Maybe<LayerColor>.None;
        }

        var layerColor = LayerColorMapping.MapToLayerColor(item);

        return layerColor;
    }

    public async Task<Maybe<LayerColor[]>> GetByIds(int[] layerColorIds)
    {
        var items = await _context.LayerColor
            .AsNoTracking()
            .Where(c => layerColorIds.Contains(c.Id))
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<LayerColor[]>.None;
        }

        var result = items
            .Select(LayerColorMapping.MapToLayerColor)
            .ToArray();

        return result;
    }

    public async Task<Result> Create(LayerColor layerColor)
    {
        var item = LayerColorMapping.MapToLayerColorEntity(layerColor);

        await _context.LayerColor.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure($"LayerColor {layerColor.Title} was not created");
    }

    public async Task<Result> Update(LayerColor layerColor)
    {
        var item = LayerColorMapping.MapToLayerColorEntity(layerColor);

        _context.LayerColor.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"LayerColor {layerColor.Title} was not updated");
    }

    public async Task<Result> Delete(int layerColorId)
    {
        var item = await _context.LayerColor.FirstOrDefaultAsync(l => l.Id == layerColorId);

        if (item == null)
        {
            return Result.Failure($"LayerColor by id: {layerColorId} does not exist");
        }

        _context.LayerColor.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"LayerColor by id: {layerColorId} was not deleted");
    }

    public async Task<Result> UpdateCandleLayerColor(int candleId, LayerColor[] layerColors)
    {
        var existingLayerColors = await _context.CandleLayerColor
            .Where(c => c.CandleId == candleId)
            .ToArrayAsync();

        var layerColorsToDelete = existingLayerColors
            .Where(el => layerColors.All(l => l.Id != el.LayerColorId))
            .ToArray();

        var layerColorsToAdd = layerColors
            .Where(l => existingLayerColors.All(el => el.LayerColorId != l.Id))
            .Select(l => new CandleEntityLayerColorEntity { CandleId = candleId, LayerColorId = l.Id })
            .ToArray();

        if (!layerColorsToDelete.Any() && !layerColorsToAdd.Any())
        {
            Result.Failure($"There are no LayerColors of candle by id: {candleId} that need to be updated");
        }

        _context.CandleLayerColor.RemoveRange(layerColorsToDelete);
        _context.CandleLayerColor.AddRange(layerColorsToAdd);

        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"LayerColors of candle by id: {candleId} were not updated");
    }
}