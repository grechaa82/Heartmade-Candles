﻿using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class SmellRepository : ISmellRepository
    {
        private readonly AdminDbContext _context;

        public SmellRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<Smell[]> GetAll()
        {
            var items = await _context.Smell
                .AsNoTracking()
                .ToArrayAsync();

            var result = items
                .Select(item => SmellMapping.MapToSmell(item))
                .ToArray();

            return result;
        }
        
        public async Task<Smell> Get(int smellId)
        {
            var item = await _context.Smell
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == smellId);

            var smell = SmellMapping.MapToSmell(item);

            return smell;
        }

        public async Task<Smell[]> GetByIds(int[] smellIds)
        {
            var items = await _context.Smell
                .AsNoTracking()
                .Where(c => smellIds.Contains(c.Id))
                .ToArrayAsync();

            var result = items
                .Select(item => SmellMapping.MapToSmell(item))
                .ToArray();

            return result;
        }

        public async Task Create(Smell smell)
        {
            var result = SmellMapping.MapToSmellEntity(smell);

            await _context.Smell.AddAsync(result);
            await _context.SaveChangesAsync();
        }
        
        public async Task Update(Smell smell)
        {
            var item = SmellMapping.MapToSmellEntity(smell);

            _context.Smell.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int smellId)
        {
            var item = await _context.Smell.FirstOrDefaultAsync(c => c.Id == smellId);

            if (item != null)
            {
                _context.Smell.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCandleSmell(int candleId, Smell[] smells)
        {
            var existingSmells = await _context.CandleSmell
                .Where(c => c.CandleId == candleId)
                .ToArrayAsync();

            var smellsToDelete = existingSmells
                .Where(es => !smells.Any(s => s.Id == es.SmellId))
                .ToArray();

            var smellsToAdd = smells
                .Where(s => !existingSmells.Any(es => es.SmellId == s.Id))
                .Select(s => new CandleEntitySmellEntity { CandleId = candleId, SmellId = s.Id })
                .ToArray();

            _context.RemoveRange(smellsToDelete);
            _context.AddRange(smellsToAdd);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> AreIdsExist(int[] ids)
        {
            foreach (var id in ids)
            {
                var exists = await _context.Smell.AnyAsync(d => d.Id == id);

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
                var exists = await _context.Smell.AnyAsync(d => d.Id == id);

                if (!exists)
                {
                    nonExistingIds.Add(id);
                }
            }

            return nonExistingIds.ToArray();
        }
    }
}
