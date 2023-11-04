﻿using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.Core.Models;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services;

public class OrderService : IOrderService
{
    private readonly IOrderNotificationHandler _orderNotificationHandler;
    private readonly IOrderRepository _orderRepository;
    private readonly IConstructorService _constructorService;
    private readonly ICalculateService _calculateService;

    public OrderService(
        IOrderRepository orderRepository, 
        IOrderNotificationHandler orderNotificationHandler, 
        IConstructorService constructorService,
        ICalculateService calculateService)
    {
        _orderRepository = orderRepository;
        _orderNotificationHandler = orderNotificationHandler;
        _constructorService = constructorService;
        _calculateService = calculateService;
    }

    public async Task<Result<Basket>> GetBasketById(string orderDetailId)
    {
        return await _orderRepository.GetBasketById(orderDetailId);
    }

    public async Task<Result<string>> CreateBasket(ConfiguredCandleFilter[] configuredCandlesFilters)
    {
        var basketItems = new List<BasketItem>();

        foreach (var configuredCandleFilter in configuredCandlesFilters)
        {
            Result<Constructor.Core.Models.CandleDetail> candleDetail = await _constructorService.GetCandleByFilter(MapToCandleDetailFilter(configuredCandleFilter));

            var configuredCandle = MapConstructorCandleDetailToOrderConfiguredCandle(candleDetail.Value);

            var price = _calculateService.CalculatePrice(configuredCandle);

            var basketItem = new BasketItem
            {
                ConfiguredCandle = configuredCandle,
                Price = price.Value,
                ConfiguredCandleFilter = configuredCandleFilter
            };

            if (basketItem.IsMatchingConfiguredCandle().IsFailure)
            {
                throw new ArgumentException();
            }

            basketItems.Add(basketItem);
        }

        var basket = new Basket
        {
            Items = basketItems.ToArray()
        };

        var basketIdResult = await _orderRepository.CreateBasket(basket);

        return basketIdResult;
    }

    public async Task<Result<Core.Models.Order>> GetOrderById(string orderId)
    {
        return await _orderRepository.GetOrderById(orderId);
    } 

    public async Task<Result<string>> CreateOrder(User user, Feedback feedback, string basketId)
    {
        var basket = await _orderRepository.GetBasketById(basketId);

        foreach (var basketItem in basket.Value.Items)
        {
            var currentStateCandleDetail =
                await _constructorService.GetCandleByFilter(MapToCandleDetailFilter(basketItem.ConfiguredCandleFilter));

            var currentStateConfiguredCandle =
                MapConstructorCandleDetailToOrderConfiguredCandle(currentStateCandleDetail.Value);

            if (basketItem.IsComparedConfiguredCandles(currentStateConfiguredCandle).IsFailure)
            {
                throw new ArgumentException();
            }
        }

        var order = new Core.Models.Order
        {
            OrderDetailId = basketId,
            User = user,
            Feedback = feedback,
            Status = OrderStatus.Assembled
        };

        return await _orderRepository.CreateOrder(order);
    }

    private ConfiguredCandle MapConstructorCandleDetailToOrderConfiguredCandle(Constructor.Core.Models.CandleDetail candleDetail)
    {
        return new ConfiguredCandle
        {
            Candle = MapConstructorCandleToOrderCandle(candleDetail.Candle),
            Decor = candleDetail.Decors.Any() 
                ? MapConstructorDecorToOrderDecor(candleDetail.Decors[0]) 
                : null,
            LayerColors = MapConstructorLayerColorsToOrderLayerColors(candleDetail.LayerColors),
            NumberOfLayer = candleDetail.Decors.Any() 
                ? MapConstructorNumberOfLayerToOrderNumberOfLayer(candleDetail.NumberOfLayers[0]) 
                : throw new InvalidCastException(),
            Smell = candleDetail.Smells.Any() 
                ? MapConstructorSmellToOrderSmell(candleDetail.Smells[0]) 
                : null,
            Wick = candleDetail.Wicks.Any() 
                ? MapConstructorWickToOrderWick(candleDetail.Wicks[0]) 
                : throw new InvalidCastException()
        };
    }

    private Core.Models.Candle MapConstructorCandleToOrderCandle(Constructor.Core.Models.Candle candle)
    {
        return new Core.Models.Candle(
            candle.Id,
            candle.Title,
            candle.Price,
            candle.WeightGrams,
            MapConstructorImageToOrderImage(candle.Images));
    }

    private Core.Models.Decor MapConstructorDecorToOrderDecor(Constructor.Core.Models.Decor decor)
    {
        return new Core.Models.Decor(decor.Id, decor.Title, decor.Price);
    }

    private Core.Models.LayerColor[] MapConstructorLayerColorsToOrderLayerColors(Constructor.Core.Models.LayerColor[] layerColor)
    {
        return layerColor
            .Select(x => new Core.Models.LayerColor(x.Id, x.Title, x.PricePerGram))
            .ToArray();
    }

    private Core.Models.NumberOfLayer MapConstructorNumberOfLayerToOrderNumberOfLayer(Constructor.Core.Models.NumberOfLayer numberOfLayer)
    {
        return new Core.Models.NumberOfLayer(numberOfLayer.Id, numberOfLayer.Number);
    }

    private Core.Models.Smell MapConstructorSmellToOrderSmell(Constructor.Core.Models.Smell smell)
    {
        return new Core.Models.Smell(smell.Id, smell.Title, smell.Price);
    }

    private Core.Models.Wick MapConstructorWickToOrderWick(Constructor.Core.Models.Wick wick)
    {
        return new Core.Models.Wick(wick.Id, wick.Title, wick.Price);
    }

    private Core.Models.Image[] MapConstructorImageToOrderImage(Constructor.Core.Models.Image[] image)
    {
        return image
            .Select(x => new Core.Models.Image(x.FileName, x.AlternativeName))
            .ToArray();
    }

    private CandleDetailFilter MapToCandleDetailFilter(ConfiguredCandleFilter filter)
    {
        return new CandleDetailFilter
        {
            CandleId = filter.CandleId,
            DecorId = filter.DecorId,
            NumberOfLayerId = filter.NumberOfLayerId,
            LayerColorIds = filter.LayerColorIds,
            SmellId = filter.SmellId,
            WickId = filter.WickId
        };
    }
}