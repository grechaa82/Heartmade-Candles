using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
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
                var candle = Candle.Create(
                    item.Title,
                    item.Description,
                    item.WeightGrams,
                    item.ImageURL,
                    item.IsActive,
                    TypeCandle.ContainerCandle,
                    item.Id);

                result.Add(candle.Value);
            }

            return result;
        }

        public async Task<Candle> Get(int id)
        {
            var item = await _context.Candle
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var candle = Candle.Create(
                item.Title,
                item.Description,
                item.WeightGrams,
                item.ImageURL,
                item.IsActive,
                TypeCandle.ContainerCandle,
                item.Id);

            return candle.Value;
        }

        public async Task Create(Candle candle)
        {
            var item = new CandleEntity()
            {
                Id = candle.Id,
                Title = candle.Title,
                Description = candle.Description,
                WeightGrams = candle.WeightGrams,
                ImageURL = candle.ImageURL,
                IsActive = candle.IsActive,
                TypeCandleId = 1,
                CreatedAt = candle.CreatedAt
            };

            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Candle candle)
        {
            var item = new CandleEntity()
            {
                Id = candle.Id,
                Title = candle.Title,
                Description = candle.Description,
                WeightGrams = candle.WeightGrams,
                ImageURL = candle.ImageURL,
                IsActive = candle.IsActive,
                TypeCandleId = 1,
                CreatedAt = candle.CreatedAt
            };

            _context.Update(item);
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
