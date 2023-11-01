using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class OrderMapping
{
    public static Core.Models.Order MapToOrder(OrderCollection orderCollection)
    {
        return new Core.Models.Order
        {
            Id = orderCollection.Id,
            OrderDetailId = orderCollection.OrderDetailId,
            OrderDetail = orderCollection.OrderDetail == null ? null : OrderDetailMapping.MapToOrderDetail(orderCollection.OrderDetail),
            User = UserMapping.MapToUser(orderCollection.User),
            Feedback = FeedbackMapping.MapToFeedback(orderCollection.Feedback),
            Status = orderCollection.Status
        };
    }

    public static Core.Models.Order MapToOrder(OrderCollection orderCollection, OrderDetailCollection orderDetailCollection)
    {
        return new Core.Models.Order
        {
            Id = orderCollection.Id,
            OrderDetailId = orderCollection.OrderDetailId,
            OrderDetail = OrderDetailMapping.MapToOrderDetail(orderDetailCollection),
            User = UserMapping.MapToUser(orderCollection.User),
            Feedback = FeedbackMapping.MapToFeedback(orderCollection.Feedback),
            Status = orderCollection.Status
        };
    }

    public static OrderCollection MapToOrderCollection(Core.Models.Order order)
    {
        return new OrderCollection
        {
            Id = order.Id,
            OrderDetailId = order.OrderDetailId,
            OrderDetail = order.OrderDetail == null ? null : OrderDetailMapping.MapToOrderDetailCollection(order.OrderDetail),
            User = UserMapping.MapToUserCollection(order.User),
            Feedback = FeedbackMapping.MapToFeedbackCollection(order.Feedback),
            Status = order.Status
        };
    }
}

