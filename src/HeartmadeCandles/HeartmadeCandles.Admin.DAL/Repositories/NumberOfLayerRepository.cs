using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class NumberOfLayerRepository : INumberOfLayerRepository
    {
        private readonly AdminDbContext _context;

        public NumberOfLayerRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<NumberOfLayer>> GetAll()
        {
            var result = new List<NumberOfLayer>();

            var items = await _context.NumberOfLayer
                .AsNoTracking()
                .ToArrayAsync();

            foreach (var item in items)
            {
                result.Add(NumberOfLayer.Create(item.Number, item.Id));
            }

            return result;
        }

        public async Task<NumberOfLayer> Get(int id)
        {
            var item = await _context.NumberOfLayer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var result = NumberOfLayer.Create(item.Number, item.Id);

            return result;
        }

        public async Task Create(NumberOfLayer numberOfLayer)
        {
            var item = new NumberOfLayerEntity()
            {
                Id = numberOfLayer.Id,
                Number = numberOfLayer.Number
            };

            await _context.NumberOfLayer.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(NumberOfLayer numberOfLayer)
        {
            var item = new NumberOfLayerEntity()
            {
                Id = numberOfLayer.Id,
                Number = numberOfLayer.Number
            };

            _context.NumberOfLayer.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.NumberOfLayer.FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                _context.NumberOfLayer.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
