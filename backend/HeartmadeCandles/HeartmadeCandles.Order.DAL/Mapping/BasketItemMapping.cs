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
            var basketItem = BasketItem.Create(
                configuredCandle: ConfiguredCandleMapping.MapToConfiguredCandle(basketItemDocument.ConfiguredCandle),
                price: basketItemDocument.Price,
                configuredCandleFilter: ConfiguredCandleFilterMapping.MapToConfiguredCandleFilter(basketItemDocument.ConfiguredCandleFilter)
            );

            orderDetailItems.Add(basketItem.Value);
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
