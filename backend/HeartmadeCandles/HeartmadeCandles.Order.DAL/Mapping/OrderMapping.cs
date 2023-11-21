using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class OrderMapping
{
    public static Core.Models.Order MapToOrder(OrderDocument orderDocument)
    {
        return new Core.Models.Order
        {
            Id = orderDocument.Id,
            BasketId = orderDocument.BasketId,
            Basket = orderDocument.Basket == null ? null : BasketMapping.MapToBasket(orderDocument.Basket),
            User = UserMapping.MapToUser(orderDocument.User),
            Feedback = FeedbackMapping.MapToFeedback(orderDocument.Feedback),
            Status = orderDocument.Status
        };
    }

    public static Core.Models.Order MapToOrder(OrderDocument orderDocument, BasketDocument basketDocument)
    {
        return new Core.Models.Order
        {
            Id = orderDocument.Id,
            BasketId = orderDocument.BasketId,
            Basket = BasketMapping.MapToBasket(basketDocument),
            User = UserMapping.MapToUser(orderDocument.User),
            Feedback = FeedbackMapping.MapToFeedback(orderDocument.Feedback),
            Status = orderDocument.Status
        };
    }

    public static OrderDocument MapToOrderDocument(Core.Models.Order order)
    {
        return new OrderDocument
        {
            Id = order.Id,
            BasketId = order.BasketId,
            Basket = order.Basket == null ? null : BasketMapping.MapToBasketDocument(order.Basket),
            User = UserMapping.MapToUserDocument(order.User),
            Feedback = FeedbackMapping.MapToFeedbackDocument(order.Feedback),
            Status = order.Status
        };
    }
}

