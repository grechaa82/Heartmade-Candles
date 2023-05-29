using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
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
    }
}
