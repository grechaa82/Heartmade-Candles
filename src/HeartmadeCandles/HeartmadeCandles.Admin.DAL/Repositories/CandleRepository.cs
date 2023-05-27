using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class CandleRepository : ICandleRepository
    {
        private readonly AdminDbContext _context;

        public CandleRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Candle>> GetAll()
        {
            var result = new List<Candle>();

            var items = await _context.Candle
                .AsNoTracking()
                .Include(c => c.TypeCandle)
                .ToListAsync();

            foreach (var item in items)
            {
                var typeCandle = TypeCandle.Create(item.TypeCandle.Title, item.TypeCandle.Id);

                var candle = Candle.Create(
                    item.Title,
                    item.Description,
                    item.Price,
                    item.WeightGrams,
                    item.ImageURL,
                    typeCandle.Value,
                    item.IsActive,
                    item.Id);

                result.Add(candle.Value);
            }

            return result;
        }

        public async Task<CandleDetail> Get(int id)
        {
            var candleDetailEntity = await _context.Candle
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Include(c => c.TypeCandle)
                .Include(d => d.CandleDecor)
                .Include(l => l.CandleLayerColor)
                .Include(n => n.CandleNumberOfLayer)
                .Include(s => s.CandleSmell)
                .Include(w => w.CandleWick)
                .ToArrayAsync();

            if (candleDetailEntity == null)
            {
                return default;
            }

            var typeCandle = TypeCandle.Create(candleDetailEntity[0].TypeCandle.Title, candleDetailEntity[0].TypeCandle.Id);

            var candle = Candle.Create(
                candleDetailEntity[0].Title,
                candleDetailEntity[0].Description,
                candleDetailEntity[0].Price,
                candleDetailEntity[0].WeightGrams,
                candleDetailEntity[0].ImageURL,
                typeCandle.Value,
                candleDetailEntity[0].IsActive,
                candleDetailEntity[0].Id);

            List<Decor> decors = new List<Decor>();
            var decorEntities = candleDetailEntity[0].CandleDecor;
            if (decorEntities != null)
            {
                foreach (var decorEntity in decorEntities)
                {
                    var item = await _context.Decor
                        .AsNoTracking()
                        .FirstAsync(d => d.Id == decorEntity.DecorId);

                    var decor = Decor.Create(
                        item.Title,
                        item.Description,
                        item.Price,
                        item.ImageURL,
                        item.IsActive,
                        item.Id);

                    decors.Add(decor.Value);
                }
            }

            List<LayerColor> layerColors = new List<LayerColor>();
            var layerColorEntities = candleDetailEntity[0].CandleLayerColor;
            if (layerColorEntities != null)
            {
                foreach (var layerColorEntity in layerColorEntities)
                {
                    var item = await _context.LayerColor
                        .AsNoTracking()
                        .FirstAsync(l => l.Id == layerColorEntity.LayerColorId);

                    var layerColor = LayerColor.Create(
                        item.Title,
                        item.Description,
                        item.PricePerGram,
                        item.ImageURL,
                        item.IsActive,
                        item.Id);

                    layerColors.Add(layerColor.Value);
                }
            }

            List<NumberOfLayer> numberOfLayers = new List<NumberOfLayer>();
            var numberOfLayerEntities = candleDetailEntity[0].CandleNumberOfLayer;
            if (numberOfLayerEntities != null)
            {
                foreach (var numberOfLayerEntity in numberOfLayerEntities)
                {
                    var item = await _context.NumberOfLayer
                        .AsNoTracking()
                        .FirstAsync(n => n.Id == numberOfLayerEntity.NumberOfLayerId);

                    var numberOfLayer = NumberOfLayer.Create(item.Number, item.Id);

                    numberOfLayers.Add(numberOfLayer.Value);
                }
            }

            List<Smell> smells = new List<Smell>();
            var smellEntities = candleDetailEntity[0].CandleSmell;
            if (smellEntities != null)
            {
                foreach (var smellEntity in smellEntities)
                {
                    var item = await _context.Smell
                        .AsNoTracking()
                        .FirstAsync(s => s.Id == smellEntity.SmellId);

                    var smell = Smell.Create(
                        item.Title,
                        item.Description,
                        item.Price,
                        item.IsActive,
                        item.Id);

                    smells.Add(smell.Value);
                }
            }

            List<Wick> wicks = new List<Wick>();
            var wickEntities = candleDetailEntity[0].CandleWick;
            if (smellEntities != null)
            {
                foreach (var wickEntity in wickEntities)
                {
                    var item = await _context.Wick
                        .AsNoTracking()
                        .FirstAsync(s => s.Id == wickEntity.WickId);

                    var wick = Wick.Create(
                        item.Title,
                        item.Description,
                        item.Price,
                        item.ImageURL,
                        item.IsActive,
                        item.Id);

                    wicks.Add(wick.Value);
                }
            }

            var candleDetail = CandleDetail.Create(candle.Value, decors, layerColors, numberOfLayers, smells, wicks);

            return candleDetail.Value;
        }

        public async Task Create(Candle candle)
        {
            var item = new CandleEntity()
            {
                Id = candle.Id,
                Title = candle.Title,
                Description = candle.Description,
                Price = candle.Price,
                WeightGrams = candle.WeightGrams,
                ImageURL = candle.ImageURL,
                IsActive = candle.IsActive,
                TypeCandleId = candle.TypeCandle.Id,
                CreatedAt = candle.CreatedAt
            };

            await _context.Candle.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Candle candle)
        {
            var item = new CandleEntity()
            {
                Id = candle.Id,
                Title = candle.Title,
                Description = candle.Description,
                Price = candle.Price,
                WeightGrams = candle.WeightGrams,
                ImageURL = candle.ImageURL,
                IsActive = candle.IsActive,
                TypeCandleId = candle.TypeCandle.Id,
                CreatedAt = candle.CreatedAt
            };

            _context.Candle.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.Candle.FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                _context.Candle.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
