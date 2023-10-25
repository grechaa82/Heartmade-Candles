using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services;

public class OrderService : IOrderService
{
    private readonly IOrderNotificationHandler _orderNotificationHandler;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository, IOrderNotificationHandler orderNotificationHandler)
    {
        _orderRepository = orderRepository;
        _orderNotificationHandler = orderNotificationHandler;
    }

    public async Task<Result<Core.Models.Order>> Get(int orderId)
    {
        var orderResult = await _orderRepository.GetOrder(orderId);

        if (orderResult.IsFailure)
        {
            return orderResult;
        }

        return orderResult.Value;
    }

    public async Task<Result<int>> CreateOrder(OrderItemFilter[] orderItemFilters)
    {
        var orderItemsResult = await _orderRepository.GetOrderItems(orderItemFilters);
        if (orderItemsResult.IsFailure)
        {
            return Result.Failure<int>(orderItemsResult.Error);
        }

        var invalidOrderItems = orderItemsResult.Value
            .Select(o => o.CheckIsOrderItemMissing())
            .ToArray();

        if (invalidOrderItems.Any(o => o.IsFailure))
        {
            return Result.Failure<int>(
                string.Join(", ", invalidOrderItems.Where(o => o.IsFailure).Select(e => e.Error)));
        }

        var orderResult = Core.Models.Order.Create("configuredCandlesString", orderItemsResult.Value, null, null, OrderStatus.Created);

        if (orderResult.IsFailure)
        {
            return Result.Failure<int>(orderResult.Error);
        }
        
        return await _orderRepository.CreateOrder(orderResult.Value);
    }


    public async Task<Result> UpdateOrderStatus(int orderId)
    {
        /*
         * Сервис для обновления статуса заказа Task<Result> UpdateOrderStatus(int orderId)
         * Нужно для обновления статуса из ТГ
         */

        return Result.Success();
    }

    public async Task<Result> CheckoutOrder(
        string configuredCandlesString,
        int orderId,
        User user,
        Feedback feedback)
    {
        var orderResult = await _orderRepository.GetOrder(orderId);

        if (orderResult.IsFailure)
        {
            return orderResult;
        }

        var newOrderResult = Core.Models.Order.Create(configuredCandlesString, orderResult.Value.OrderItems, user, feedback, OrderStatus.Issued);

        var updateResult = await _orderRepository.UpdateOrder(newOrderResult.Value);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        var notificationResult = await _orderNotificationHandler.OnCreateOrder(newOrderResult.Value);

        if (notificationResult.IsFailure)
        {
            return Result.Failure(notificationResult.Error);
        }

        return Result.Success();
    }
}