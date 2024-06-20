using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IConstructorService _constructorService;

    public OrderService(
        IOrderRepository orderRepository, 
        IBasketRepository basketRepository,
        IConstructorService constructorService)
    {
        _orderRepository = orderRepository;
        _basketRepository = basketRepository;
        _constructorService = constructorService;
    }

    public async Task<Result<Core.Models.Order>> GetOrderById(string orderId)
    {
        var orderMaybe = await _orderRepository.GetOrderById(orderId);
        
        return orderMaybe.HasValue
            ? Result.Success(orderMaybe.Value)
            : Result.Failure<Core.Models.Order>($"Order by id: {orderId} does not exist");
    }

    public async Task<(Maybe<Core.Models.Order[]>, long)> GetOrderByStatusWithTotalOrders(OrderStatus status, int pageSige, int pageIndex)
    {
        return await _orderRepository.GetOrderByStatusWithTotalOrders(status, pageSige, pageIndex);
    }

    public async Task<Result<string>> CreateOrder(Feedback? feedback, string basketId)
    {
        var basket = await _basketRepository.GetBasketById(basketId);

        foreach (var basketItem in basket.Value.Items)
        {
            var currentStateCandleDetail =
                await _constructorService.GetCandleByFilter(OrderMapping.MapToCandleDetailFilter(basketItem.ConfiguredCandleFilter));

            var currentStateConfiguredCandle =
                OrderMapping.MapConstructorCandleDetailToOrderConfiguredCandle(currentStateCandleDetail.Value);

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
            Feedback = feedback,
            Status = OrderStatus.Created
        };

        var createOrderResult = await _orderRepository.CreateOrder(order);
        if (createOrderResult.IsFailure)
        {
            return Result.Failure<string>(createOrderResult.Error);
        }

        return Result.Success(createOrderResult.Value);
    }

    public async Task<Result> UpdateOrderStatus(string orderId, OrderStatus status)
    {
        var orderResult =  await _orderRepository.GetOrderById(orderId);

        if (orderResult.HasValue)
        {
            return Result.Failure($"Order by id: {orderId} does not exist");
        }

        var newOrder = new Core.Models.Order
        {
            Basket = orderResult.Value.Basket,
            BasketId = orderResult.Value.BasketId,
            Feedback = orderResult.Value.Feedback,
            Status = status,
            CreatedAt = orderResult.Value.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _orderRepository.UpdateOrder(newOrder);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }
}