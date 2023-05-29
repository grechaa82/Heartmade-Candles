﻿using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;


namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class CandleRepository : ICandleRepository
    {
        private readonly AdminDbContext _context;

        public CandleRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Candle>> GetAll()
        {
            var result = new List<Candle>();

            var items = await _context.Candle
                .AsNoTracking()
                .Include(c => c.TypeCandle)
                .ToListAsync();

            foreach (var item in items)
            {
                var typeCandle = TypeCandleMapping.MapToCandleType(item.TypeCandle);
                var candle = CandleMapping.MapToCandle(item, typeCandle);

                result.Add(candle);
            }

            return result;
        }

        public async Task<CandleDetail> Get(int id)
        {
            var candleDetailEntity = await _context.Candle
                .AsNoTracking()
                .Include(t => t.TypeCandle)
                .Include(cd => cd.CandleDecor).ThenInclude(d => d.Decor)
                .Include(cl => cl.CandleLayerColor).ThenInclude(l => l.LayerColor)
                .Include(cn => cn.CandleNumberOfLayer).ThenInclude(n => n.NumberOfLayer)
                .Include(cs => cs.CandleSmell).ThenInclude(s => s.Smell)
                .Include(cw => cw.CandleWick).ThenInclude(w => w.Wick)
                .FirstOrDefaultAsync(c => c.Id == id);

            var typeCandle = TypeCandleMapping.MapToCandleType(candleDetailEntity.TypeCandle);
            var candle = CandleMapping.MapToCandle(candleDetailEntity, typeCandle);

            var decors = candleDetailEntity.CandleDecor
                .Select(cd => DecorMapping.MapToDecor(cd.Decor))
                .ToList();
            var layerColors = candleDetailEntity.CandleLayerColor
                .Select(clc => LayerColorMapping.MapToLayerColor(clc.LayerColor))
                .ToList();
            var numberOfLayers = candleDetailEntity.CandleNumberOfLayer
                .Select(cnol => NumberOfLayerMapping.MapToNumberOfLayer(cnol.NumberOfLayer))
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

        public async Task Delete(int id)
        {
            var item = await _context.Candle.FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                _context.Candle.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
