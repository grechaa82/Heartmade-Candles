using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
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

        public async Task<TypeCandle[]> GetAll()
        {
            var items = await _context.TypeCandle
                .AsNoTracking()
                .ToArrayAsync();

            var result = items
                .Select(item => TypeCandleMapping.MapToCandleType(item))
                .ToArray();

            return result;
        }

        public async Task<TypeCandle> Get(int id)
        {
            var item = await _context.TypeCandle
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var typeCandle = TypeCandleMapping.MapToCandleType(item);

            return typeCandle;
        }

        public async Task Create(TypeCandle typeCandle)
        {
            var item = TypeCandleMapping.MapToTypeCandleEntity(typeCandle);

            await _context.TypeCandle.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TypeCandle typeCandle)
        {
            var item = TypeCandleMapping.MapToTypeCandleEntity(typeCandle);

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
