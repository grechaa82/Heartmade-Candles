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
            Feedback = FeedbackMapping.MapToFeedback(orderDocument.Feedback),
            Status = orderDocument.Status,
            CreatedAt = orderDocument.CreatedAt,
            UpdatedAt = orderDocument.UpdatedAt
        };
    }

    public static Core.Models.Order MapToOrder(OrderDocument orderDocument, BasketDocument basketDocument)
    {
        return new Core.Models.Order
        {
            Id = orderDocument.Id,
            BasketId = orderDocument.BasketId,
            Basket = BasketMapping.MapToBasket(basketDocument),
            Feedback = FeedbackMapping.MapToFeedback(orderDocument.Feedback),
            Status = orderDocument.Status,
            CreatedAt = orderDocument.CreatedAt,
            UpdatedAt = orderDocument.UpdatedAt
        };
    }

    public static OrderDocument MapToOrderDocument(Core.Models.Order order)
    {
        return new OrderDocument
        {
            Id = order.Id,
            BasketId = order.BasketId,
            Basket = order.Basket == null ? null : BasketMapping.MapToBasketDocument(order.Basket),
            Feedback = FeedbackMapping.MapToFeedbackDocument(order.Feedback),
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}

