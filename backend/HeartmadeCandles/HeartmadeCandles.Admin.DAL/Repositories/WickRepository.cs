﻿using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class WickRepository : IWickRepository
    {
        private readonly AdminDbContext _context;

        public WickRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<Wick[]> GetAll()
        {
            var items = await _context.Wick
                .AsNoTracking()
                .ToArrayAsync();

            var result = items
                .Select(item => WickMapping.MapToWick(item))
                .ToArray();

            return result;
        }

        public async Task<Wick> Get(int wickId)
        {
            var item = await _context.Wick
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == wickId);

            var wick = WickMapping.MapToWick(item);

            return wick;
        }

        public async Task<Wick[]> GetByIds(int[] wickIds)
        {
            var items = await _context.Wick
                .AsNoTracking()
                .Where(c => wickIds.Contains(c.Id))
                .ToArrayAsync();

            var result = items
                .Select(item => WickMapping.MapToWick(item))
                .ToArray();

            return result;
        }

        public async Task Create(Wick wick)
        {
            var item = WickMapping.MapToWickEntity(wick);

            await _context.Wick.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Wick wick)
        {
            var item = WickMapping.MapToWickEntity(wick);

            _context.Wick.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int wickId)
        {
            var item = await _context.Wick.FirstOrDefaultAsync(c => c.Id == wickId);

            if (item != null)
            {
                _context.Wick.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCandleWick(int candleId, Wick[] wicks)
        {
            var existingWicks = await _context.CandleWick
                .Where(c => c.CandleId == candleId)
                .ToArrayAsync();

            var wicksToDelete = existingWicks
                .Where(ew => !wicks.Any(w => w.Id == ew.WickId))
                .ToArray();

            var wicksToAdd = wicks
                .Where(w => !existingWicks.Any(ew => ew.WickId == w.Id))
                .Select(w => new CandleEntityWickEntity { CandleId = candleId, WickId = w.Id })
                .ToArray();

            _context.RemoveRange(wicksToDelete);
            _context.AddRange(wicksToAdd);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> AreIdsExist(int[] ids)
        {
            foreach (var id in ids)
            {
                var exists = await _context.Wick.AnyAsync(d => d.Id == id);

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
                var exists = await _context.Wick.AnyAsync(d => d.Id == id);

                if (!exists)
                {
                    nonExistingIds.Add(id);
                }
            }

            return nonExistingIds.ToArray();
        }
    }
}