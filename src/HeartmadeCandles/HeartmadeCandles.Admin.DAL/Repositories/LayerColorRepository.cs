using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class LayerColorRepository : ILayerColorRepository
    {
        private readonly AdminDbContext _context;

        public LayerColorRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<LayerColor>> GetAll()
        {
            var result = new List<LayerColor>();

            var items = await _context.LayerColor
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in items)
            {
                var layerColor = LayerColorMapping.MapToLayerColor(item);

                result.Add(layerColor);
            }

            return result;
        }

        public async Task<LayerColor> Get(int id)
        {
            var item = await _context.LayerColor
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
            
            var layerColor = LayerColorMapping.MapToLayerColor(item);

            return layerColor;
        }

        public async Task Create(LayerColor layerColor)
        {
            var item = LayerColorMapping.MapToLayerColorEntity(layerColor);

            await _context.LayerColor.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(LayerColor layerColor)
        {
            var item = LayerColorMapping.MapToLayerColorEntity(layerColor);

            _context.LayerColor.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.LayerColor.FirstOrDefaultAsync(l => l.Id == id);

            if (item != null)
            {
                _context.LayerColor.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
