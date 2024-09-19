using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories;

public class CandleRepository : ICandleRepository
{
    private readonly AdminDbContext _context;

    public CandleRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<Maybe<Candle[]>> GetAll(TypeCandle? typeCandle, PaginationSettings pagination)
    {
        IQueryable<CandleEntity> query = _context.Candle.AsNoTracking().Include(c => c.TypeCandle);

        if (typeCandle != null)
        {
            query = query.Where(item => item.TypeCandleId == typeCandle.Id);
        }

        var items = await query
            .OrderBy(item => item.Id)
            .Skip(pagination.PageSize * pagination.PageIndex)
            .Take(pagination.PageSize)
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<Candle[]>.None;
        }

        var result = items.Select(item =>
        {
            var mappedTypeCandle = TypeCandleMapping.MapToCandleType(item.TypeCandle);
            return CandleMapping.MapToCandle(item, mappedTypeCandle);
        }).ToArray();

        return result;
    }

    public async Task<Maybe<Candle>> GetById(int candleId)
    {
        var item = await _context.Candle
            .AsNoTracking()
            .Include(c => c.TypeCandle)
            .FirstOrDefaultAsync(c => c.Id == candleId);

        if (item == null)
        {
            return Maybe<Candle>.None;
        }

        var typeCandle = TypeCandleMapping.MapToCandleType(item.TypeCandle);

        var result = CandleMapping.MapToCandle(item, typeCandle);

        return result;
    }

    public async Task<Maybe<CandleDetail>> GetCandleDetailById(int candleId)
    {
        var candleDetailEntity = await _context.Candle
            .AsNoTracking()
            .Include(t => t.TypeCandle)
            .Include(cd => cd.CandleDecor)
            .ThenInclude(d => d.Decor)
            .Include(cl => cl.CandleLayerColor)
            .ThenInclude(l => l.LayerColor)
            .Include(cn => cn.CandleNumberOfLayer)
            .ThenInclude(n => n.NumberOfLayer)
            .Include(cs => cs.CandleSmell)
            .ThenInclude(s => s.Smell)
            .Include(cw => cw.CandleWick)
            .ThenInclude(w => w.Wick)
            .FirstOrDefaultAsync(c => c.Id == candleId);

        if (candleDetailEntity == null)
        {
            return Maybe<CandleDetail>.None;
        }

        var typeCandle = TypeCandleMapping.MapToCandleType(candleDetailEntity.TypeCandle);
        var candle = CandleMapping.MapToCandle(candleDetailEntity, typeCandle);

        var decors = candleDetailEntity.CandleDecor
            .Select(cd => DecorMapping.MapToDecor(cd.Decor))
            .ToList();
        var layerColors = candleDetailEntity.CandleLayerColor
            .Select(cl => LayerColorMapping.MapToLayerColor(cl.LayerColor))
            .ToList();
        var numberOfLayers = candleDetailEntity.CandleNumberOfLayer
            .Select(cn => NumberOfLayerMapping.MapToNumberOfLayer(cn.NumberOfLayer))
            .ToList();
        var smells = candleDetailEntity.CandleSmell
            .Select(cs => SmellMapping.MapToSmell(cs.Smell))
            .ToList();
        var wicks = candleDetailEntity.CandleWick
            .Select(cw => WickMapping.MapToWick(cw.Wick))
            .ToList();

        var candleDetail = CandleDetail.Create(
            candle,
            decors,
            layerColors,
            numberOfLayers,
            smells,
            wicks);

        return candleDetail.Value;
    }

    public async Task<Result> Create(Candle candle)
    {
        var item = CandleMapping.MapToCandleEntity(candle);

        await _context.Candle.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure($"Candle {candle.Title} was not created");
    }

    public async Task<Result> Update(Candle candle)
    {
        var item = CandleMapping.MapToCandleEntity(candle);

        _context.Candle.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"Candle {candle.Title} was not updated");
    }

    public async Task<Result> Delete(int candleId)
    {
        var item = await _context.Candle.FirstOrDefaultAsync(c => c.Id == candleId);

        if (item == null)
        {
            return Result.Failure($"Candle by id: {candleId} does not exist");
        }

        _context.Candle.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"Candle by id: {candleId} was not deleted");
    }
}