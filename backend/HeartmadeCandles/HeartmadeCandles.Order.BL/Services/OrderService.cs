using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services;

public class OrderService : IOrderService
{
    private readonly IOrderNotificationHandler _orderNotificationHandler;
    private readonly IOrderRepository _orderRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IConstructorService _constructorService;
    public OrderService(
        IOrderRepository orderRepository, 
        IBasketRepository basketRepository,
        IOrderNotificationHandler orderNotificationHandler, 
        IConstructorService constructorService)
    {
        _orderRepository = orderRepository;
        _basketRepository = basketRepository;
        _orderNotificationHandler = orderNotificationHandler;
        _constructorService = constructorService;
    }

    public async Task<Result<Core.Models.Order>> GetOrderById(string orderId)
    {
        var orderMaybe = await _orderRepository.GetOrderById(orderId);
        
        return orderMaybe.HasValue
            ? Result.Success(orderMaybe.Value)
            : Result.Failure<Core.Models.Order>($"Order by id: {orderId} does not exist");
    } 

    public async Task<Result<string>> CreateOrder(User user, Feedback feedback, string basketId)
    {
        var basket = await _basketRepository.GetBasketById(basketId);

        foreach (var basketItem in basket.Value.Items)
        {
            var currentStateCandleDetail =
                await _constructorService.GetCandleByFilter(Mapping.MapToCandleDetailFilter(basketItem.ConfiguredCandleFilter));

            var currentStateConfiguredCandle =
                Mapping.MapConstructorCandleDetailToOrderConfiguredCandle(currentStateCandleDetail.Value);

            var isComparedConfiguredCandles = basketItem.Compare(currentStateConfiguredCandle);
            if (isComparedConfiguredCandles.IsFailure)
            {
                return Result.Failure<string>(isComparedConfiguredCandles.Error);
            }
        }

        var order = new Core.Models.Order
        {
            Basket = basket.Value,
            BasketId = basketId,
            User = user,
            Feedback = feedback,
            Status = OrderStatus.Assembled
        };

        var createOrderResult = await _orderRepository.CreateOrder(order);
        if (createOrderResult.IsFailure)
        {
            return Result.Failure<string>(createOrderResult.Error);
        }

        var onCreateOrderResult = await _orderNotificationHandler.OnCreateOrder(order);
        if (onCreateOrderResult.IsFailure)
        {
            return Result.Failure<string>(onCreateOrderResult.Error);
        }

        return Result.Success(createOrderResult.Value);
    }
}