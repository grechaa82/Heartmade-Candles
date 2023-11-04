using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class BasketItemMapping
{
    public static BasketItem[] MapToBasketItem(BasketItemDocument[] basketItemsDocument)
    {
        var orderDetailItems = new List<BasketItem>();

        foreach (var basketItemDocument in basketItemsDocument)
        {
            var basketItem = new BasketItem
            {
                ConfiguredCandle = ConfiguredCandleMapping.MapToConfiguredCandle(basketItemDocument.ConfiguredCandle),
                Price = basketItemDocument.Price,
                ConfiguredCandleFilter = ConfiguredCandleFilterMapping.MapToConfiguredCandleFilter(basketItemDocument.ConfiguredCandleFilter)
            };

            orderDetailItems.Add(basketItem);
        }

        return orderDetailItems.ToArray();
    }

    public static BasketItemDocument[] MapToBasketItemDocument(BasketItem[] basketItems)
    {
        var basketItemsDocuments = new List<BasketItemDocument>();

        foreach (var basketItem in basketItems)
        {
            var basketItemDocument = new BasketItemDocument
            {
                ConfiguredCandle = ConfiguredCandleMapping.MapToConfiguredCandleDocument(basketItem.ConfiguredCandle),
                Price = basketItem.Price,
                Quantity = basketItem.Quantity,
                ConfiguredCandleFilter = ConfiguredCandleFilterMapping.MapToConfiguredCandleFilterDocument(basketItem.ConfiguredCandleFilter)
            };

            basketItemsDocuments.Add(basketItemDocument);
        }

        return basketItemsDocuments.ToArray();
    }
}
