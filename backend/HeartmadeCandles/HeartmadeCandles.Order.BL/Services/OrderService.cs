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

    public async Task<Result<OrderItem[]>> Get(OrderItemFilter[] orderItemFilters)
    {
        var orderItemsResult = await _orderRepository.Get(orderItemFilters);
        if (orderItemsResult.IsFailure)
        {
            return orderItemsResult;
        }

        var invalidOrderItems = orderItemsResult.Value
            .Select(o => o.CheckIsOrderItemMissing())
            .ToArray();
        if (invalidOrderItems.Any(o => o.IsFailure))
        {
            return Result.Failure<OrderItem[]>(
                string.Join(", ", invalidOrderItems.Where(o => o.IsFailure).Select(e => e.Error)));
        }

        return orderItemsResult.Value;
    }

    public async Task<Result> CreateOrder(
        string configuredCandlesString,
        OrderItemFilter[] OrderItemFilters,
        User user,
        Feedback feedback)
    {
        var result = Result.Success();

        var orderItemsResult = await _orderRepository.Get(OrderItemFilters);
        if (orderItemsResult.IsFailure)
        {
            return orderItemsResult;
        }

        var invalidOrderItems = orderItemsResult.Value
            .Select(o => o.CheckIsOrderItemMissing())
            .ToArray();
        if (invalidOrderItems.Any(o => o.IsFailure))
        {
            return Result.Failure(
                string.Join(
                    ", ",
                    invalidOrderItems
                        .Where(o => o.IsFailure)
                        .Select(e => e.Error)));
        }

        var orderResutl = Core.Models.Order.Create(
            configuredCandlesString,
            orderItemsResult.Value,
            user,
            feedback);
        if (orderResutl.IsFailure)
        {
            return Result.Failure(orderResutl.Error);
        }

        var isMessageSend = await _orderNotificationHandler.OnCreateOrder(orderResutl.Value);
        if (isMessageSend.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }
}