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

    public async Task<Result<OrderDetail>> GetOrderDetailById(string orderDetailId)
    {
        return await _orderRepository.GetOrderDetailById(orderDetailId);
    }

    public async Task<Result<string>> CreateOrderDetail(OrderDetailItem[] orderItems)
    {
        // TODO: Check the existence of these objects by id in the database
        var orderDetail = new OrderDetail
        {
            Items = orderItems,
        };

        return await _orderRepository.CreateOrderDetail(orderDetail);
    }

    public async Task<Result<Core.Models.Order>> GetOrderById(string orderId)
    {
        return await _orderRepository.GetOrderById(orderId);
    } 

    public async Task<Result<string>> CreateOrder(User user, Feedback feedback, string orderDetailId)
    {
        var order = new Core.Models.Order
        {
            OrderDetailId = orderDetailId,
            User = user,
            Feedback = feedback,
            Status = OrderStatus.Assembled,
        };

        return await _orderRepository.CreateOrder(order);
    }
}