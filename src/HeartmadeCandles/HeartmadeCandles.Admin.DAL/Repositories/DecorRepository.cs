using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
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
                var decor = Decor.Create(
                    item.Title,
                    item.Description,
                    item.Price,
                    item.ImageURL,
                    item.IsActive,
                    item.Id);

                result.Add(decor.Value);
            }

            return result;
        }
        
        public async Task<Decor> Get(int id)
        {
            var item = await _context.Decor
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var decor = Decor.Create(
                item.Title,
                item.Description,
                item.Price,
                item.ImageURL,
                item.IsActive,
                item.Id);

            return decor.Value;
        }
        
        public async Task Create(Decor decor)
        {
            var item = new DecorEntity()
            {
                Id = decor.Id,
                Title = decor.Title,
                Description = decor.Description,
                Price = decor.Price,
                ImageURL = decor.ImageURL,
                IsActive = decor.IsActive,
                
            };

            await _context.Decor.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Decor decor)
        {
            var item = new DecorEntity()
            {
                Id = decor.Id,
                Title = decor.Title,
                Description = decor.Description,
                Price = decor.Price,
                ImageURL = decor.ImageURL,
                IsActive = decor.IsActive,

            };

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
