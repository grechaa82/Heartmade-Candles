namespace HeartmadeCandles.Bot.BL;

public class BotMapping
{
    public static Core.Models.Order MapOrderToBotOrder(Order.Core.Models.Order order)
    {
        return new Core.Models.Order
        {
            Id = order.Id,
            Basket = MapOrderBasketToBotBasket(order.Basket),
            Feedback = MapOrderFeedbackToBotFeedback(order.Feedback),
            Status = MapOrderOrderStatusToBotOrderStatus(order.Status),
        };
    }

    public static Core.Models.Order[] MapOrderToBotOrder(Order.Core.Models.Order[] orders)
    {
        return orders
            .Select(MapOrderToBotOrder)
            .ToArray();
    }

    public static Core.Models.Basket MapOrderBasketToBotBasket(Order.Core.Models.Basket basket)
    {
        return new Core.Models.Basket
        {
            Id = basket.Id!, 
            Items = basket.Items
                .Select(MapOrderBasketItemToBotBasketItem)
                .ToArray(), 
            TotalPrice = basket.TotalPrice, 
            TotalQuantity = basket.TotalQuantity, 
            FilterString = basket.FilterString
        };
    }

    public static Core.Models.Feedback MapOrderFeedbackToBotFeedback(Order.Core.Models.Feedback feedback)
    {
        return new Core.Models.Feedback
        {
            TypeFeedback = feedback.TypeFeedback,
            UserName = feedback.UserName
        };
    }

    public static Core.Models.OrderStatus MapOrderOrderStatusToBotOrderStatus(Order.Core.Models.OrderStatus orderStatus)
    {
        return orderStatus switch
        {
            Order.Core.Models.OrderStatus.Created => Core.Models.OrderStatus.Created,
            Order.Core.Models.OrderStatus.Confirmed => Core.Models.OrderStatus.Confirmed,
            Order.Core.Models.OrderStatus.Placed => Core.Models.OrderStatus.Placed,
            Order.Core.Models.OrderStatus.Paid => Core.Models.OrderStatus.Paid,
            Order.Core.Models.OrderStatus.InProgress => Core.Models.OrderStatus.InProgress,
            Order.Core.Models.OrderStatus.Packed => Core.Models.OrderStatus.Packed,
            Order.Core.Models.OrderStatus.InDelivery => Core.Models.OrderStatus.InDelivery,
            Order.Core.Models.OrderStatus.Completed => Core.Models.OrderStatus.Completed,
            Order.Core.Models.OrderStatus.Cancelled => Core.Models.OrderStatus.Cancelled,
            _ => throw new InvalidCastException(),
        };
    }

    public static Order.Core.Models.OrderStatus MapBotOrderStatusToOrderOrderStatus(Core.Models.OrderStatus status)
    {
        return status switch
        {
            Core.Models.OrderStatus.Created => Order.Core.Models.OrderStatus.Created,
            Core.Models.OrderStatus.Confirmed => Order.Core.Models.OrderStatus.Confirmed,
            Core.Models.OrderStatus.Placed => Order.Core.Models.OrderStatus.Placed,
            Core.Models.OrderStatus.Paid => Order.Core.Models.OrderStatus.Paid,
            Core.Models.OrderStatus.InProgress => Order.Core.Models.OrderStatus.InProgress,
            Core.Models.OrderStatus.Packed => Order.Core.Models.OrderStatus.Packed,
            Core.Models.OrderStatus.InDelivery => Order.Core.Models.OrderStatus.InDelivery,
            Core.Models.OrderStatus.Completed => Order.Core.Models.OrderStatus.Completed,
            Core.Models.OrderStatus.Cancelled => Order.Core.Models.OrderStatus.Cancelled,
            _ => throw new InvalidCastException(),
        };
    }

    public static Core.Models.BasketItem MapOrderBasketItemToBotBasketItem(Order.Core.Models.BasketItem basketItem)
    {
        return new Core.Models.BasketItem
        {
            ConfiguredCandle = MapOrderConfiguredCandleToBotConfiguredCandle(basketItem.ConfiguredCandle),
            Price =  basketItem.Price,
            Quantity =  basketItem.Quantity,
            ConfiguredCandleFilterString = basketItem.ConfiguredCandleFilter.FilterString 
        };
    }

    public static Core.Models.ConfiguredCandle MapOrderConfiguredCandleToBotConfiguredCandle(Order.Core.Models.ConfiguredCandle configuredCandle)
    {
        return new Core.Models.ConfiguredCandle
        {
            Candle = MapOrderCandleToBotCandle(configuredCandle.Candle),
            Decor = configuredCandle.Decor != null
                ? MapOrderDecorToBotDecor(configuredCandle.Decor)
                : null,
            LayerColors = MapOrderLayerColorsToBotLayerColors(configuredCandle.LayerColors),
            NumberOfLayer = MapOrderNumberOfLayerToBotNumberOfLayer(configuredCandle.NumberOfLayer),
            Smell = configuredCandle.Smell != null
                ? MapOrderSmellToBotSmell(configuredCandle.Smell)
                : null,
            Wick = MapOrderWickToBotWick(configuredCandle.Wick)
        };
    }

    public static Core.Models.Candle MapOrderCandleToBotCandle(Order.Core.Models.Candle candle)
    {
        return new Core.Models.Candle
        {
            Id = candle.Id,
            Title = candle.Title,
            Price = candle.Price,
            WeightGrams = candle.WeightGrams
        };
    }

    public static Core.Models.Decor MapOrderDecorToBotDecor(Order.Core.Models.Decor decor)
    {
        return new Core.Models.Decor
        {
            Id = decor.Id, 
            Title = decor.Title,
            Price = decor.Price
        };
    }

    public static Core.Models.LayerColor[] MapOrderLayerColorsToBotLayerColors(Order.Core.Models.LayerColor[] layerColor)
    {
        return layerColor
            .Select(x => new Core.Models.LayerColor
                {
                    Id = x.Id, 
                    Title = x.Title,
                    PricePerGram = x.PricePerGram
                })
            .ToArray();
    }

    public static Core.Models.NumberOfLayer MapOrderNumberOfLayerToBotNumberOfLayer(Order.Core.Models.NumberOfLayer numberOfLayer)
    {
        return new Core.Models.NumberOfLayer
        {
            Id =numberOfLayer.Id,
            Number = numberOfLayer.Number
        };
    }

    public static Core.Models.Smell MapOrderSmellToBotSmell(Order.Core.Models.Smell smell)
    {
        return new Core.Models.Smell
        {
            Id = smell.Id, 
            Title = smell.Title, 
            Price = smell.Price
        };
    }

    public static Core.Models.Wick MapOrderWickToBotWick(Order.Core.Models.Wick wick)
    {
        return new Core.Models.Wick
        {
            Id = wick.Id, 
            Title = wick.Title, 
            Price = wick.Price
        };
    }
}
