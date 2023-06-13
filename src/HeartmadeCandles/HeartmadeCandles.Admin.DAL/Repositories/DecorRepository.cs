using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class DecorRepository : IDecorRepository
    {
        private readonly AdminDbContext _context;

        public DecorRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<Decor>> GetAll()
        {
            var result = new List<Decor>();

            var items = await _context.Decor
                .AsNoTracking()
                .ToArrayAsync();

            foreach (var item in items)
            {
                var decor = DecorMapping.MapToDecor(item);

                result.Add(decor);
            }

            return result;
        }

        public async Task<Decor> Get(int id)
        {
            var item = await _context.Decor
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var decor = DecorMapping.MapToDecor(item);

            return decor;
        }

        public async Task<Decor[]> GetByIds(int[] ids)
        {
            var items = await _context.Decor
                .AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .ToArrayAsync();

            var result = items.Select(item => DecorMapping.MapToDecor(item)).ToArray();

            return result;
        }

        public async Task Create(Decor decor)
        {
            var item = DecorMapping.MapToDecorEntity(decor);

            await _context.Decor.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Decor decor)
        {
            var item = DecorMapping.MapToDecorEntity(decor);

            _context.Decor.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.Decor.FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                _context.Decor.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCandleDecor(int candleId, List<Decor> decors)
        {
            var existingDecors = await _context.CandleDecor
                .Where(c => c.CandleId == candleId)
                .ToArrayAsync();

            var decorsToDelete = existingDecors
                .Where(ed => !decors.Any(d => d.Id == ed.DecorId))
                .ToArray();

            var decorsToAdd = decors
                .Where(d => !existingDecors.Any(ed => ed.DecorId == d.Id))
                .Select(d => new CandleEntityDecorEntity { CandleId = candleId, DecorId = d.Id })
                .ToArray();

            _context.RemoveRange(decorsToDelete);
            _context.AddRange(decorsToAdd);

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
}
