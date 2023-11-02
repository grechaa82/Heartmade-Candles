using CSharpFunctionalExtensions;
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

    public async Task<Result<string>> CreateBasket(ConfiguredCandleFilter[] candleDetailsFilters)
    {
        /*
         * 1. Получить ConfiguredCandle[] из _constructorService.GetCandleDetailByFilter(candleDetailsFilters[i])
         * 2. Проверить что на соответсвие
         * 3. Создать BasketItem[]
         * 4. Создать Basket
         * 5. Сохранить в _orderRepository.CreateBasket(Basket basket)
         * 6. Вернуть Basket.Id 
         */

        var basketItems = new List<BasketItem>();

        foreach (var candleDetailFilter in candleDetailsFilters)
        {
            Result<Constructor.Core.Models.CandleDetail> candleDetail = await _constructorService.GetCandleByFilter(
                candleDetailFilter.CandleId,
                candleDetailFilter.DecorId,
                candleDetailFilter.NumberOfLayerId,
                candleDetailFilter.LayerColorIds,
                candleDetailFilter.SmellId,
                candleDetailFilter.WickId);

            var configuredCandle = MapConstructorCandleDetailToOrderConfiguredCandle(candleDetail.Value);

            var price = await _calculateService.CalculatePrice(configuredCandle);

            var basketItem = new BasketItem
            {
                ConfiguredCandle = configuredCandle,
                Price = price.Value,
                ConfiguredCandleFilter = candleDetailFilter
            };

            if (basketItem.CheckConsistencyConfiguredCandle().IsFailure)
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
        /*
         * 1. Получить корзину из Mongo var basket = _orderRepository.GetBasketById(basketId)
         * 2. Получить ConfiguredCandle[] по настройке из корзины var currentStateConfiguredCandle _constructorService.GetCandleDetailByFilter(basket.Items[i].ConfiguredCandleFilter)
         * 3. Проверить если currentStateConfiguredCandle != basket.Items[i] то нужно сказать, что состояние этой корзины устарело, измените конфигурацию свечи
         * 4. Создать Order[]
         * 5. Сохранить в _orderRepository.CreateOrder(Order order)
         * 6. Вернуть Order.Id
         */

        var order = new Core.Models.Order
        {
            OrderDetailId = basketId,
            User = user,
            Feedback = feedback,
            Status = OrderStatus.Assembled,
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
                ? MapConstructoroWickToOrderWick(candleDetail.Wicks[0]) 
                : throw new InvalidCastException()
        };
    }

    private Core.Models.Candle MapConstructorCandleToOrderCandle(Constructor.Core.Models.Candle candle)
    {
        throw new NotImplementedException();
    }

    private Core.Models.Decor MapConstructorDecorToOrderDecor(Constructor.Core.Models.Decor decor)
    {
        throw new NotImplementedException();
    }

    private Core.Models.LayerColor[] MapConstructorLayerColorsToOrderLayerColors(Constructor.Core.Models.LayerColor[] layerColor)
    {
        throw new NotImplementedException();
    }

    private Core.Models.NumberOfLayer MapConstructorNumberOfLayerToOrderNumberOfLayer(Constructor.Core.Models.NumberOfLayer numberOfLayer)
    {
        throw new NotImplementedException();
    }

    private Core.Models.Smell MapConstructorSmellToOrderSmell(Constructor.Core.Models.Smell smell)
    {
        throw new NotImplementedException();
    }

    private Core.Models.Wick MapConstructoroWickToOrderWick(Constructor.Core.Models.Wick wick)
    {
        throw new NotImplementedException();
    }

    private Core.Models.Image[] MapConstructorImageToOrderImage(Constructor.Core.Models.Image[] image)
    {
        throw new NotImplementedException(); ;
    }
}