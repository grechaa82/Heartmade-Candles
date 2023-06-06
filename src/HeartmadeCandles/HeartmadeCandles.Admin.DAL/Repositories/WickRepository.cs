using HeartmadeCandles.Admin.Core.Interfaces;
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

        public async Task<List<Wick>> GetAll()
        {
            var result = new List<Wick>();

            var items = await _context.Wick
                .AsNoTracking()
                .ToArrayAsync();

            foreach (var item in items)
            {
                var wick = WickMapping.MapToWick(item);

                result.Add(wick);
            }

            return result;
        }

        public async Task<Wick> Get(int id)
        {
            var item = await _context.Wick
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var wick = WickMapping.MapToWick(item);

            return wick;
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

        public async Task Delete(int id)
        {
            var item = await _context.Wick.FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                _context.Wick.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCandleWick(int candleId, List<Wick> wicks)
        {
            var wicksToDelete = _context.CandleWick.Where(c => c.CandleId == candleId).ToList();
            _context.RemoveRange(wicksToDelete);
            await _context.SaveChangesAsync();

            var wicksToAdd = new List<CandleEntityWickEntity>();
            foreach (var wick in wicks)
            {
                var wickEntity = new CandleEntityWickEntity()
                {
                    CandleId = candleId,
                    WickId= wick.Id
                };

                wicksToAdd.Add(wickEntity);
            }

            _context.AddRange(wicksToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
