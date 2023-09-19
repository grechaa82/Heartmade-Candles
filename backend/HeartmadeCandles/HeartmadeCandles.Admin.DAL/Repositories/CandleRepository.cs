using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
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

    public async Task<Candle[]> GetAll()
    {
        var items = await _context.Candle
            .AsNoTracking()
            .Include(c => c.TypeCandle)
            .ToArrayAsync();

        var result = items.Select(
            item =>
            {
                var typeCandle = TypeCandleMapping.MapToCandleType(item.TypeCandle);
                return CandleMapping.MapToCandle(item, typeCandle);
            }).ToArray();

        return result;
    }

    public async Task<Candle?> GetById(int candleId)
    {
        var item = await _context.Candle
            .AsNoTracking()
            .Include(c => c.TypeCandle)
            .FirstOrDefaultAsync(c => c.Id == candleId);

        if (item == null)
        {
            return null;
        }

        var typeCandle = TypeCandleMapping.MapToCandleType(item.TypeCandle);

        var result = CandleMapping.MapToCandle(item, typeCandle);

        return result;
    }

    public async Task<CandleDetail> GetCandleDetailById(int candleId)
    {
        var candleDetailEntity = await _context.Candle
            .AsNoTracking()
            .Include(t => t.TypeCandle)
            .Include(cd => cd.CandleDecor).ThenInclude(d => d.Decor)
            .Include(cl => cl.CandleLayerColor).ThenInclude(l => l.LayerColor)
            .Include(cn => cn.CandleNumberOfLayer).ThenInclude(n => n.NumberOfLayer)
            .Include(cs => cs.CandleSmell).ThenInclude(s => s.Smell)
            .Include(cw => cw.CandleWick).ThenInclude(w => w.Wick)
            .FirstOrDefaultAsync(c => c.Id == candleId);

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

    public async Task Create(Candle candle)
    {
        var item = CandleMapping.MapToCandleEntity(candle);

        await _context.Candle.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Candle candle)
    {
        var item = CandleMapping.MapToCandleEntity(candle);

        _context.Candle.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int candleId)
    {
        var item = await _context.Candle.FirstOrDefaultAsync(c => c.Id == candleId);

        if (item != null)
        {
            _context.Candle.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}