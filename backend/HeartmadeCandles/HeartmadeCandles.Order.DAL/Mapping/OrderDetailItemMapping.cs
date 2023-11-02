using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class OrderDetailItemMapping
{
    public static BasketItem[] MapToOrderDetailItem(OrderDetailItemCollection[] orderDetailItemsCollection)
    {
        var orderDetailItems = new List<BasketItem>();

        foreach (var orderDetailItemCollection in orderDetailItemsCollection)
        {
            var orderDetailItem = new BasketItem
            {
                Candle = CandleMapping.MapToCandle(orderDetailItemCollection.Candle),
                Decor = DecorMapping.MapToDecor(orderDetailItemCollection.Decor ),
                LayerColors = LayerColorMapping.MapToLayerColors(orderDetailItemCollection.LayerColors),
                NumberOfLayer = NumberOfLayerMapping.MapToNumberOfLayer(orderDetailItemCollection.NumberOfLayer),
                Smell = SmellMapping.MapToSmell(orderDetailItemCollection.Smell),
                Wick = WickMapping.MapToWick(orderDetailItemCollection.Wick),
                Quantity = orderDetailItemCollection.Quantity,
                ConfigurationString = orderDetailItemCollection.ConfigurationString
            };

            orderDetailItems.Add(orderDetailItem);
        }

        return orderDetailItems.ToArray();
    }

    public static OrderDetailItemCollection[] MapToOrderDetailItemCollection(BasketItem[] orderDetailItems)
    {
        var orderDetailItemsCollection = new List<OrderDetailItemCollection>();

        foreach (var orderDetailItem in orderDetailItems)
        {
            var orderDetailItemCollection = new OrderDetailItemCollection
            {
                Candle = CandleMapping.MapToCandleCollection(orderDetailItem.Candle),
                Decor = DecorMapping.MapToDecorCollection(orderDetailItem.Decor),
                LayerColors = LayerColorMapping.MapToLayerColorsCollection(orderDetailItem.LayerColors),
                NumberOfLayer = NumberOfLayerMapping.MapToNumberOfLayerCollection(orderDetailItem.NumberOfLayer),
                Smell = SmellMapping.MapToSmellCollection(orderDetailItem.Smell),
                Wick = WickMapping.MapToWickCollection(orderDetailItem.Wick),
                Price = orderDetailItem.Price,
                Quantity = orderDetailItem.Quantity,
                ConfigurationString = orderDetailItem.ConfigurationString
            };

            orderDetailItemsCollection.Add(orderDetailItemCollection);
        }

        return orderDetailItemsCollection.ToArray();
    }
}
