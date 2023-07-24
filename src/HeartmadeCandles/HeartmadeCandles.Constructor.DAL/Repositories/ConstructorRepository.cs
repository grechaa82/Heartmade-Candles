using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.Core.Models;
using HeartmadeCandles.Constructor.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Constructor.DAL.Repositories
{
    public class ConstructorRepository : IConstructorRepository
    {
        private readonly ConstructorDbContext _context;

        public ConstructorRepository(ConstructorDbContext context)
        {
            _context = context;
        }

        public async Task<CandleTypeWithCandles[]> GetCandles()
        {
            var items = await _context.Candle
                .AsNoTracking()
                .Where(c => c.IsActive)
                .GroupBy(c => c.TypeCandle.Title)
                .ToArrayAsync();

            var result = items.Select(c => new CandleTypeWithCandles()
            {
                Type = c.Key,
                Candles = c.Select(candle => MapToCandle(candle)).ToArray()
            }).ToArray();

            return result;
        }

        public async Task<Maybe<CandleDetail>> GetCandleById(int candleId)
        {
            var candleDetailEntity = await _context.Candle
                .AsNoTracking()
                .Include(t => t.TypeCandle)
                .Include(cd => cd.CandleDecor).ThenInclude(d => d.Decor)
                .Include(cl => cl.CandleLayerColor).ThenInclude(l => l.LayerColor)
                .Include(cn => cn.CandleNumberOfLayer).ThenInclude(n => n.NumberOfLayer)
                .Include(cs => cs.CandleSmell).ThenInclude(s => s.Smell)
                .Include(cw => cw.CandleWick).ThenInclude(w => w.Wick)
                .FirstOrDefaultAsync(c => c.Id == candleId);

            if (candleDetailEntity == null)
            {
                return Maybe.None;
            }

            var candle = MapToCandle(candleDetailEntity);

            var decors = candleDetailEntity.CandleDecor
                .Select(cd => MapToDecor(cd.Decor))
                .ToArray();
            var layerColors = candleDetailEntity.CandleLayerColor
                .Select(cl => MapToLayerColor(cl.LayerColor))
                .ToArray();
            var numberOfLayers = candleDetailEntity.CandleNumberOfLayer
                .Select(cn => MapToNumberOfLayer(cn.NumberOfLayer))
                .ToArray();
            var smells = candleDetailEntity.CandleSmell
                .Select(cs => MapToSmell(cs.Smell))
                .ToArray();
            var wicks = candleDetailEntity.CandleWick
                .Select(cw => MapToWick(cw.Wick))
                .ToArray();

            return new CandleDetail()
            {
                Candle = candle,
                Decors = decors,
                LayerColors = layerColors,
                NumberOfLayers = numberOfLayers,
                Smells = smells,
                Wicks = wicks
            };
        }

        private Candle MapToCandle(CandleEntity candleEntity)
        {
            return new Candle()
            {
                Id = candleEntity.Id,
                Title = candleEntity.Title,
                Description = candleEntity.Description,
                Price = candleEntity.Price,
                WeightGrams = candleEntity.WeightGrams,
                Images = MapToImage(candleEntity.Images)
            };
        }

        private TypeCandle MapToCandleType(TypeCandleEntity typeCandleEntity)
        {
            return new TypeCandle()
            {
                Id = typeCandleEntity.Id,
                Title = typeCandleEntity.Title
            };
        }

        private Decor MapToDecor(DecorEntity decorEntity)
        {
            return new Decor()
            {
                Id = decorEntity.Id,
                Title = decorEntity.Title,
                Description = decorEntity.Description,
                Price = decorEntity.Price,
                Images = MapToImage(decorEntity.Images)
            };
        }

        private LayerColor MapToLayerColor(LayerColorEntity layerColorEntity)
        {
            return new LayerColor()
            {
                Id = layerColorEntity.Id,
                Title = layerColorEntity.Title,
                Description = layerColorEntity.Description,
                PricePerGram = layerColorEntity.PricePerGram,
                Images = MapToImage(layerColorEntity.Images)
            };
        }

        private NumberOfLayer MapToNumberOfLayer(NumberOfLayerEntity numberOfLayerEntity)
        {
            return new NumberOfLayer() 
            { 
                Id = numberOfLayerEntity.Id,
                Number = numberOfLayerEntity.Number
            };
        }

        private Smell MapToSmell(SmellEntity smellEntity)
        {
            return new Smell()
            {
                Id = smellEntity.Id,
                Title = smellEntity.Title,
                Description = smellEntity.Description,
                Price = smellEntity.Price
            };
        }

        public Wick MapToWick(WickEntity wickEntity)
        {
            return new Wick()
            {
                Id = wickEntity.Id,
                Title = wickEntity.Title,
                Description = wickEntity.Description,
                Price = wickEntity.Price,
                Images = MapToImage(wickEntity.Images)
            };
        }

        private Image[] MapToImage(ImageEntity[] imageEntities)
        {
            var images = new List<Image>();

            foreach (var imageEntity in imageEntities)
            {
                var image = new Image()
                {
                    FileName = imageEntity.FileName,
                    AlternativeName = imageEntity.AlternativeName
                };
                images.Add(image);
            }

            return images.ToArray();
        }
    }
}
