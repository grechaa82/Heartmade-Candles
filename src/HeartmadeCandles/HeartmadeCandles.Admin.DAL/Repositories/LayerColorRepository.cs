using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
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

            foreach ( var item in items )
            {
                result.Add(new LayerColor(
                    item.Title,
                    item.Description,
                    item.PricePerGram,
                    item.ImageURL,
                    item.IsActive,
                    item.Id));
            }

            return result;
        }

        public async Task<LayerColor> Get(int id)
        {
            var item = await _context.LayerColor.FirstOrDefaultAsync(l => l.Id == id);
            
            var layerColor = new LayerColor(
                item.Title,
                item.Description,
                item.PricePerGram,
                item.ImageURL,
                item.IsActive,
                item.Id);

            return layerColor;
        }

        public async Task Create(LayerColor layerColor)
        {
            var item = new LayerColorEntity()
            {
                Id = layerColor.Id,
                Title = layerColor.Title,
                Description = layerColor.Description,
                PricePerGram = layerColor.PricePerGram,
                ImageURL = layerColor.ImageURL,
                IsActive = layerColor.IsActive,
            };

            await _context.LayerColor.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(LayerColor layerColor)
        {
            var item = new LayerColorEntity()
            {
                Id = layerColor.Id,
                Title = layerColor.Title,
                Description = layerColor.Description,
                PricePerGram = layerColor.PricePerGram,
                ImageURL = layerColor.ImageURL,
                IsActive = layerColor.IsActive,
            };

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
