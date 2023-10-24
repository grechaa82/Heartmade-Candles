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

    public async Task<Result<OrderItem[]>> Get(int orderId)
    {
        var orderItems = new List<OrderItem>();

        /*
         * Order receipt process
         */

        return Result.Success(orderItems.ToArray());
    }

    public async Task<Result<int>> CreateOrder(OrderItemFilter[] orderItemFilters)
    {
        /*
         * Creating OrderItem
         * Creating Order
         * Return Order.Id
         */

        return Result.Success(0);
    }

    private Candle MapToCandle(CandleEntity candleEntity)
    {
        return new Candle(
            candleEntity.Id,
            candleEntity.Title,
            candleEntity.Description,
            candleEntity.Price,
            candleEntity.WeightGrams,
            MapToImage(candleEntity.Images));
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
            MapToImage(layerColorEntity.Images));
    }

    private NumberOfLayer MapToNumberOfLayer(NumberOfLayerEntity numberOfLayerEntity)
    {
        return new NumberOfLayer(
            numberOfLayerEntity.Id,
            numberOfLayerEntity.Number);
    }

    private Smell MapToSmell(SmellEntity smellEntity)
    {
        return new Smell(
            smellEntity.Id,
            smellEntity.Title,
            smellEntity.Description,
            smellEntity.Price);
    }

    private Wick MapToWick(WickEntity wickEntity)
    {
        return new Wick(
            wickEntity.Id,
            wickEntity.Title,
            wickEntity.Description,
            wickEntity.Price,
            MapToImage(wickEntity.Images));
    }

    private Image[] MapToImage(ImageEntity[] imageEntities)
    {
        var images = new List<Image>();

        foreach (var imageEntity in imageEntities)
        {
            var image = new Image(
                imageEntity.FileName,
                imageEntity.AlternativeName);

            images.Add(image);
        }

        return images.ToArray();
    }
}