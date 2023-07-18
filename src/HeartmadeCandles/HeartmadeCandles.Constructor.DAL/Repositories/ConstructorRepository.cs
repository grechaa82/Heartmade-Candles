using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Constructor.DAL.Repositories
{
    public class ConstructorRepository : IConstructorRepository
    {
        private readonly ConstructorDbContext _context;

        public ConstructorRepository(ConstructorDbContext context)
        {
            _context = context;
        }

        public async Task<CandleTypeWithCandles[]> GetAll()
        {
            var items = await _context.Candle
                .AsNoTracking()
                .Where(c => c.IsActive)
                .GroupBy(c => c.TypeCandle.Title)
                .ToArrayAsync();

            var result = items.Select(c => new CandleTypeWithCandles()
            {
                Type = c.Key,
                Candles = c.Select(candle => new Candle()
                {
                    Id = candle.Id,
                    Title = candle.Title,
                    Description = candle.Description,
                    Price = candle.Price,
                    WeightGrams = candle.WeightGrams,
                    ImageURL = candle.ImageURL,
                }).ToArray()
            }).ToArray();

            return result;
        }
    }
}
