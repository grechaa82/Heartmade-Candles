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

        public async Task<Candle> Get(int id)
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

        public async Task Create(Candle candle)
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

        public async Task Update(Candle candle)
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

        public async Task Delete(int id)
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
