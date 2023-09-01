using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Order.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CandleDetailWithQuantityAndPrice[]>> Get(CandleDetailIdsWithQuantity[] arrayCandleDetailIdsWithQuantity)
        {
            var result = Result.Success();

            var candleDetailWithQuantityAndPrice = new List<CandleDetailWithQuantityAndPrice>();

            foreach (var item in arrayCandleDetailIdsWithQuantity)
            {
                var candleDetailEntity = await _context.Candle
                    .AsNoTracking()
                    .Include(t => t.TypeCandle)
                    .Include(cd => cd.CandleDecor.Where(d => d.Decor.Id == item.DecorId && d.Decor.IsActive))
                        .ThenInclude(d => d.Decor)
                    .Include(cl => cl.CandleLayerColor.Where(l => item.LayerColorIds.Contains(l.LayerColor.Id) && l.LayerColor.IsActive))
                        .ThenInclude(l => l.LayerColor)
                    .Include(cn => cn.CandleNumberOfLayer.Where(n => n.NumberOfLayer.Id == item.NumberOfLayerId))
                        .ThenInclude(n => n.NumberOfLayer)
                    .Include(cs => cs.CandleSmell.Where(s => s.Smell.Id == item.SmellId && s.Smell.IsActive))
                        .ThenInclude(s => s.Smell)
                    .Include(cw => cw.CandleWick.Where(w => w.Wick.Id == item.WickId && w.Wick.IsActive))
                        .ThenInclude(w => w.Wick)
                    .FirstOrDefaultAsync(c => c.Id == item.CandleId && c.IsActive);

                if (candleDetailEntity == null)
                {
                    result = Result.Combine(
                       result,
                       Result.Failure<CandleDetailWithQuantityAndPrice[]>($"'{item.CandleId}' does not exist"));
                    continue;
                }

                var candle = MapToCandle(candleDetailEntity);

                var decors = candleDetailEntity.CandleDecor
                    .Select(cd => MapToDecor(cd.Decor))
                    .FirstOrDefault();
                var layerColors = candleDetailEntity.CandleLayerColor
                    .Select(cl => MapToLayerColor(cl.LayerColor))
                    .ToArray();
                var numberOfLayers = candleDetailEntity.CandleNumberOfLayer
                    .Select(cn => MapToNumberOfLayer(cn.NumberOfLayer))
                    .First();
                var smells = candleDetailEntity.CandleSmell
                    .Select(cs => MapToSmell(cs.Smell))
                    .FirstOrDefault();
                var wicks = candleDetailEntity.CandleWick
                    .Select(cw => MapToWick(cw.Wick))
                    .First();

                var candleDetail = new CandleDetail(
                    candle,
                    decors,
                    layerColors,
                    numberOfLayers,
                    smells,
                    wicks
                );

                candleDetailWithQuantityAndPrice.Add(new CandleDetailWithQuantityAndPrice(candleDetail, item.Quantity));
            }

            if (result.IsFailure)
            {
                return Result.Failure<CandleDetailWithQuantityAndPrice[]>(result.Error);
            }

            return Result.Success(candleDetailWithQuantityAndPrice.ToArray());
        }

        private Candle MapToCandle(CandleEntity candleEntity)
        {
            return new Candle(
                candleEntity.Id,
                candleEntity.Title,
                candleEntity.Description,
                candleEntity.Price,
                candleEntity.WeightGrams,
                MapToImage(candleEntity.Images)
            );
        }

        private Decor MapToDecor(DecorEntity decorEntity)
        {
            return new Decor(
                decorEntity.Id, 
                decorEntity.Title, 
                decorEntity.Description, 
                decorEntity.Price, 
                MapToImage(decorEntity.Images));
        }

        private LayerColor MapToLayerColor(LayerColorEntity layerColorEntity)
        {
            return new LayerColor(
                layerColorEntity.Id,
                layerColorEntity.Title,
                layerColorEntity.Description,
                layerColorEntity.PricePerGram,
                MapToImage(layerColorEntity.Images)
            );
        }

        private NumberOfLayer MapToNumberOfLayer(NumberOfLayerEntity numberOfLayerEntity)
        {
            return new NumberOfLayer(
                numberOfLayerEntity.Id,
                numberOfLayerEntity.Number
            );
        }

        private Smell MapToSmell(SmellEntity smellEntity)
        {
            return new Smell(
                smellEntity.Id,
                smellEntity.Title,
                smellEntity.Description,
                smellEntity.Price
            );
        }

        public Wick MapToWick(WickEntity wickEntity)
        {
            return new Wick(
                wickEntity.Id,
                wickEntity.Title,
                wickEntity.Description,
                wickEntity.Price,
                MapToImage(wickEntity.Images)
            );
        }

        private Image[] MapToImage(ImageEntity[] imageEntities)
        {
            var images = new List<Image>();

            foreach (var imageEntity in imageEntities)
            {
                var image = new Image(
                    imageEntity.FileName,
                    imageEntity.AlternativeName
                );
                images.Add(image);
            }

            return images.ToArray();
        }
    }
}
