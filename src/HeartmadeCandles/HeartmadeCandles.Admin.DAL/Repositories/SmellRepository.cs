using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class SmellRepository : ISmellRepository
    {
        private readonly AdminDbContext _context;

        public SmellRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<Smell>> GetAll()
        {
            var result = new List<Smell>();

            var items = await _context.Smell
                .AsNoTracking()
                .ToArrayAsync();

            foreach (var item in items)
            {
                result.Add(Smell.Create(
                    item.Title,
                    item.Description,
                    item.Price,
                    item.IsActive,
                    item.Id));
            }

            return result;
        }
        
        public async Task<Smell> Get(int id)
        {
            var item = await _context.Smell
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var result = Smell.Create(
                item.Title,
                item.Description,
                item.Price,
                item.IsActive,
                item.Id);

            return result;
        }
        
        public async Task Create(Smell smell)
        {
            var result = new SmellEntity()
            {
                Id = smell.Id,
                Title = smell.Title,
                Description = smell.Description,
                Price = smell.Price,
                IsActive = smell.IsActive
            };

            await _context.Smell.AddAsync(result);
            await _context.SaveChangesAsync();
        }
        
        public async Task Update(Smell smell)
        {
            var result = new SmellEntity()
            {
                Id = smell.Id,
                Title = smell.Title,
                Description = smell.Description,
                Price = smell.Price,
                IsActive = smell.IsActive,
            };

            _context.Smell.Update(result);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var result = await _context.Smell.FirstOrDefaultAsync(c => c.Id == id);

            if (result != null)
            {
                _context.Smell.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}
