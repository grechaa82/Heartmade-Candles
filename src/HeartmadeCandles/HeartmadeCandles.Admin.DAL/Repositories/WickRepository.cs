using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
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
                result.Add(new Wick(
                    item.Title,
                    item.Description,
                    item.Price,
                    item.ImageURL,
                    item.IsActive,
                    item.Id));
            }

            return result;
        }

        public async Task<Wick> Get(int id)
        {
            var item = await _context.Wick
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var wick = new Wick(
                item.Title,
                item.Description,
                item.Price,
                item.ImageURL,
                item.IsActive,
                item.Id);

            return wick;
        }

        public async Task Create(Wick wick)
        {
            var item = new WickEntity()
            {
                Id = wick.Id,
                Title = wick.Title,
                Description = wick.Description,
                Price = wick.Price,
                ImageURL = wick.ImageURL,
                IsActive = wick.IsActive,

            };

            await _context.Wick.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Wick wick)
        {
            var item = new WickEntity()
            {
                Id = wick.Id,
                Title = wick.Title,
                Description = wick.Description,
                Price = wick.Price,
                ImageURL = wick.ImageURL,
                IsActive = wick.IsActive,

            };

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
    }
}
