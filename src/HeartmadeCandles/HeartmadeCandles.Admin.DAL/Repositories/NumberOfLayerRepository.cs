using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Mapping;
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
                var numberOfLayer = NumberOfLayerMapping.MapToNumberOfLayer(item);

                result.Add(numberOfLayer);
            }

            return result;
        }

        public async Task<NumberOfLayer> Get(int id)
        {
            var item = await _context.NumberOfLayer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var numberOfLayer = NumberOfLayerMapping.MapToNumberOfLayer(item);

            return numberOfLayer;
        }

        public async Task Create(NumberOfLayer numberOfLayer)
        {
            var item = NumberOfLayerMapping.MapToNumberOfLayerEntity(numberOfLayer);

            await _context.NumberOfLayer.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(NumberOfLayer numberOfLayer)
        {
            var item = NumberOfLayerMapping.MapToNumberOfLayerEntity(numberOfLayer);

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
