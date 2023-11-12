using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;
using MongoDB.Bson;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class BasketMapping
{
    public static Basket MapToBasket(BasketDocument basketDocument)
    {
        return new Basket
        {
            Id = basketDocument.Id,
            Items = BasketItemMapping.MapToBasketItem(basketDocument.Items),
            FilterString = basketDocument.FilterString,
        };
    }

    public static BasketDocument MapToBasketDocument(Basket basket)
    {
        return new BasketDocument
        {
            Id = basket.Id == null ? ObjectId.GenerateNewId().ToString() : basket.Id,
            Items = BasketItemMapping.MapToBasketItemDocument(basket.Items),
            TotalPrice = basket.TotalPrice,
            TotalQuantity = basket.TotalQuantity,
            FilterString = basket.FilterString,
        };
    }
}

