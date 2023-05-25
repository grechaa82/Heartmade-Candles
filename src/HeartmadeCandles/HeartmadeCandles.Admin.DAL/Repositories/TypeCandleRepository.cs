using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class TypeCandleRepository : ITypeCandleRepository
    {
        private readonly AdminDbContext _context;

        public TypeCandleRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<TypeCandle>> GetAll()
        {
            var result = new List<TypeCandle>();

            var items = await _context.TypeCandle
                .AsNoTracking()
                .ToArrayAsync();

            foreach (var item in items)
            {
                var typeCandle = TypeCandle.Create(item.Title, item.Id);

                result.Add(typeCandle.Value);
            }

            return result;
        }

        public async Task<TypeCandle> Get(int id)
        {
            var item = await _context.TypeCandle
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var typeCandle = TypeCandle.Create(item.Title, item.Id);

            return typeCandle.Value;
        }

        public async Task Create(TypeCandle typeCandle)
        {
            var item = new TypeCandleEntity()
            {
                Id = typeCandle.Id,
                Title = typeCandle.Title
            };

            await _context.TypeCandle.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TypeCandle typeCandle)
        {
            var item = new TypeCandleEntity()
            {
                Id = typeCandle.Id,
                Title = typeCandle.Title
            };

            _context.TypeCandle.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.TypeCandle.FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                _context.TypeCandle.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
