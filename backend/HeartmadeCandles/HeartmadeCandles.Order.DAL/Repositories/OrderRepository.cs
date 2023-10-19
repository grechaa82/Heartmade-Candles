using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Order.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Result<OrderItem[]>> Get(OrderItemFilter[] orderItemFilters)
    {
        var result = Result.Success();

        var orderItems = new List<OrderItem>();

        foreach (var orderItemFilter in orderItemFilters)
        {
            var candleDetailEntity = await _context.Candle
                .AsNoTracking()
                .Include(t => t.TypeCandle)
                .Include(cd => cd.CandleDecor.Where(d => d.Decor.Id == orderItemFilter.DecorId && d.Decor.IsActive))
                .ThenInclude(d => d.Decor)
                .Include(
                    cl => cl.CandleLayerColor.Where(
                        l => orderItemFilter.LayerColorIds.Contains(l.LayerColor.Id) && l.LayerColor.IsActive))
                .ThenInclude(l => l.LayerColor)
                .Include(cn => cn.CandleNumberOfLayer.Where(n => n.NumberOfLayer.Id == orderItemFilter.NumberOfLayerId))
                .ThenInclude(n => n.NumberOfLayer)
                .Include(cs => cs.CandleSmell.Where(s => s.Smell.Id == orderItemFilter.SmellId && s.Smell.IsActive))
                .ThenInclude(s => s.Smell)
                .Include(cw => cw.CandleWick.Where(w => w.Wick.Id == orderItemFilter.WickId && w.Wick.IsActive))
                .ThenInclude(w => w.Wick)
                .FirstOrDefaultAsync(c => c.Id == orderItemFilter.CandleId && c.IsActive);

            if (candleDetailEntity == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem[]>($"Candle by id: {orderItemFilter.CandleId} does not exist"));
                continue;
            }

            var candle = MapToCandle(candleDetailEntity);

            var decors = candleDetailEntity.CandleDecor
                .Select(cd => MapToDecor(cd.Decor))
                .FirstOrDefault();
            var layerColorsDictionary =
                candleDetailEntity.CandleLayerColor.ToDictionary(x => x.LayerColor.Id, x => x.LayerColor);
            var layerColors = orderItemFilter.LayerColorIds
                .Select(
                    x => layerColorsDictionary.TryGetValue(x, out var layerColor) ? MapToLayerColor(layerColor) : null)
                .Where(x => x != null)
                .ToArray();
            var numberOfLayers = candleDetailEntity.CandleNumberOfLayer
                .Select(cn => MapToNumberOfLayer(cn.NumberOfLayer))
                .FirstOrDefault();
            var smells = candleDetailEntity.CandleSmell
                .Select(cs => MapToSmell(cs.Smell))
                .FirstOrDefault();
            var wicks = candleDetailEntity.CandleWick
                .Select(cw => MapToWick(cw.Wick))
                .FirstOrDefault();

            if (numberOfLayers == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem[]>(
                        $"NumberOfLayer by id: {orderItemFilter.NumberOfLayerId} does not exist"));
                continue;
            }

            if (!layerColors.Any())
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem[]>("LayerColors does not exist"));
                continue;
            }

            if (wicks == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<OrderItem[]>($"Wick by id: {orderItemFilter.WickId} does not exist"));
                continue;
            }

            var candleDetail = new CandleDetail(
                candle,
                decors,
                layerColors!,
                numberOfLayers,
                smells,
                wicks
            );

            orderItems.Add(OrderItem.Create(candleDetail, orderItemFilter.Quantity, orderItemFilter).Value);
        }

        if (result.IsFailure)
        {
            return Result.Failure<OrderItem[]>(result.Error);
        }

        return Result.Success(orderItems.ToArray());
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

    private Wick MapToWick(WickEntity wickEntity)
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