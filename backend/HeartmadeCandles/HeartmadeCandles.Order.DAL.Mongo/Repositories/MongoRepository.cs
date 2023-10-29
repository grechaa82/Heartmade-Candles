using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using MongoDB.Driver;
using HeartmadeCandles.Order.DAL.Mongo.Collections;
using Microsoft.Extensions.Logging;

namespace HeartmadeCandles.Order.DAL.Mongo.Repositories;

public class MongoRepository : IMongoRepository
{
    private readonly IMongoCollection<OrderCollection> _orderCollection;
    private readonly IMongoCollection<OrderDetailCollection> _orderDetailCollection;
    private readonly ILogger<MongoRepository> _logger;

    public MongoRepository(IMongoDatabase mongoDatabase, ILogger<MongoRepository> logger)
    {
        _orderCollection = mongoDatabase.GetCollection<OrderCollection>("order");
        _orderDetailCollection = mongoDatabase.GetCollection<OrderDetailCollection>("orderDetail");
        _logger = logger;
    }

    public async Task<Result<string>> CreateOrderDetail(OrderDetail orderDetail)
    {
        var orderDetailItemsCollection = MapToOrderDetailItemCollection(orderDetail.Items);

        var orderDetailCollection = new OrderDetailCollection()
        {
            Items = orderDetailItemsCollection,
            TotalPrice = orderDetail.TotalPrice,
            TotalQuantity = orderDetail.TotalQuantity,
            TotalConfigurationString = orderDetail.TotalConfigurationString
        };

        _logger.LogError($"OrderDetail OrderDetail.Id: {orderDetail.Id}");
        _logger.LogError($"orderDetailCollection.Id: {orderDetailCollection.Id}, orderDetailCollection.TotalPrice {orderDetailCollection.TotalPrice}");

        await _orderDetailCollection.InsertOneAsync(orderDetailCollection);
        return Result.Success(orderDetailCollection.Id);
    }

    public async Task<Result<OrderDetail>> GetOrderDetailById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CreateOrder(OrderV2 order)
    {
        throw new NotImplementedException();
    }

    private OrderDetailCollection MapToOrderDetailCollection(OrderDetail orderDetail)
    {
        return new OrderDetailCollection
        {
            Id = orderDetail.Id,
            Items = MapToOrderDetailItemCollection(orderDetail.Items),
            TotalPrice = orderDetail.TotalPrice,
            TotalQuantity = orderDetail.TotalQuantity,
            TotalConfigurationString = orderDetail.TotalConfigurationString
        };
    }

    private OrderDetailItemCollection[] MapToOrderDetailItemCollection(OrderDetailItemV2[] orderDetailItems)
    {
        var orderDetailItemsCollection = new List<OrderDetailItemCollection>();

        foreach (var orderDetailItem in orderDetailItems)
        {
            var orderDetailItemCollection = new OrderDetailItemCollection
            {
                Candle = MapToCandleCollection(orderDetailItem.Candle),
                Decor = MapToDecorCollection(orderDetailItem.Decor),
                LayerColors = MapToLayerColorsCollection(orderDetailItem.LayerColors),
                NumberOfLayer = MapToNumberOfLayerCollection(orderDetailItem.NumberOfLayer),
                Smell = MapToSmellCollection(orderDetailItem.Smell),
                Wick = MapToWickCollection(orderDetailItem.Wick),
                Price = orderDetailItem.Price,
                Quantity = orderDetailItem.Quantity,
                ConfigurationString = orderDetailItem.ConfigurationString
            };

            orderDetailItemsCollection.Add(orderDetailItemCollection);
        }

        return orderDetailItemsCollection.ToArray();
    }

    private CandleCollection MapToCandleCollection(Candle candle)
    {
        return new CandleCollection
        {
            Id = candle.Id,
            Title = candle.Title,
            Description = candle.Description,
            Price = candle.Price,
            WeightGrams = candle.WeightGrams,
            Images = MapToImageCollection(candle.Images)
        };
    }

    private DecorCollection MapToDecorCollection(Decor decor)
    {
        return new DecorCollection
        {
            Id = decor.Id,
            Title = decor.Title,
            Description = decor.Description,
            Price = decor.Price,
            Images = MapToImageCollection(decor.Images)
        };
    }

    private LayerColorCollection[] MapToLayerColorsCollection(LayerColor[] layerColors)
    {
        var layerColorsCollection = new List<LayerColorCollection>();

        foreach (var layerColor in layerColors)
        {
            var layerColorCollection = new LayerColorCollection
            {
                Id = layerColor.Id,
                Title = layerColor.Title,
                Description = layerColor.Description,
                PricePerGram = layerColor.PricePerGram,
                Images = MapToImageCollection(layerColor.Images)
            };

            layerColorsCollection.Add(layerColorCollection);
        }

        return layerColorsCollection.ToArray();
    }

    private NumberOfLayerCollection MapToNumberOfLayerCollection(NumberOfLayer numberOfLayer)
    {
        return new NumberOfLayerCollection
        {
            Id = numberOfLayer.Id,
            Number = numberOfLayer.Number
        };
    }

    private SmellCollection MapToSmellCollection(Smell smell)
    {
        return new SmellCollection
        {
            Id = smell.Id,
            Title = smell.Title,
            Description = smell.Description,
            Price = smell.Price
        };
    }

    private WickCollection MapToWickCollection(Wick wick)
    {
        return new WickCollection
        {
            Id = wick.Id,
            Title = wick.Title,
            Description = wick.Description,
            Price = wick.Price,
            Images = MapToImageCollection(wick.Images)
        };
    }

    private ImageCollection[] MapToImageCollection(Image[] images)
    {
        var imageCollections = new List<ImageCollection>();

        foreach (var image in images)
        {
            var imageCollection = new ImageCollection
            {
                FileName = image.FileName,
                AlternativeName = image.AlternativeName
            };

            imageCollections.Add(imageCollection);
        }

        return imageCollections.ToArray();
    }
}