using HeartmadeCandles.Modules.Admin.Core.Interfaces;
using HeartmadeCandles.Modules.Admin.Core.Models;
using HeartmadeCandles.Modules.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Modules.Admin.DAL.Repositories
{
    public class CandleRepository : ICandleRepository
    {
        private readonly ApplicationDbContext _context;

        public CandleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Candle>> GetAll()
        {
            using (_context)
            {
                var result = new List<Candle>();

                var items = await _context.Candle
                    .AsNoTracking()
                    .Include(c => c.TypeCandle)
                    .ToListAsync();

                foreach (var item in items)
                {
                    result.Add(new Candle(
                        item.Title,
                        item.Description,
                        item.ImageURL,
                        item.WeightGrams,
                        item.IsActive,
                        TypeCandle.ContainerCandle,
                        item.Id));
                }

                return result;
            }
        }

        public async Task<Candle> Get(int id)
        {
            using (_context)
            {
                var item = await _context.Candle.FirstOrDefaultAsync(c => c.Id == id);

                var candle = new Candle(
                    item.Title,
                    item.Description,
                    item.ImageURL,
                    item.WeightGrams,
                    item.IsActive,
                    TypeCandle.ContainerCandle,
                    item.Id);

                return candle;
            }
        }

        public async Task Create(Candle candle)
        {
            using (_context)
            {
                var item = new CandleEntity() 
                {
                    Id = candle.Id,
                    Title = candle.Title,
                    Description = candle.Description,
                    ImageURL = candle.ImageURL,
                    WeightGrams = candle.WeightGrams,
                    IsActive = candle.IsActive,
                    TypeCandleId = 1,
                    CreatedAt = candle.CreatedAt
                };

                _context.AddAsync(item);
                _context.SaveChanges();
            }
        }

        public async Task Update(Candle candle)
        {
            using (_context)
            {
                var item = new CandleEntity()
                {
                    Id = candle.Id,
                    Title = candle.Title,
                    Description = candle.Description,
                    ImageURL = candle.ImageURL,
                    WeightGrams = candle.WeightGrams,
                    IsActive = candle.IsActive,
                    TypeCandleId = 1,
                    CreatedAt = candle.CreatedAt
                };

                _context.Update(item);
                _context.SaveChanges();
            }
        }

        public async Task Delete(int id)
        {
            using (_context)
            {
                var item = await _context.Candle.FirstOrDefaultAsync(c => c.Id == id);

                if (item != null)
                {
                    _context.Candle.Remove(item);
                    _context.SaveChanges();
                }
            }
        }
    }
}
