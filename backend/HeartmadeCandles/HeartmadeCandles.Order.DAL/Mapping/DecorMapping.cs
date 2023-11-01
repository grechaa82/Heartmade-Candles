using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class DecorMapping
{
    public static Decor MapToDecor(DecorCollection decorCollection)
    {
        return new Decor(
            decorCollection.Id,
            decorCollection.Title,
            decorCollection.Price
        );
    }

    public static DecorCollection MapToDecorCollection(Decor decor)
    {
        return new DecorCollection
        {
            Id = decor.Id,
            Title = decor.Title,
            Price = decor.Price
        };
    }
}

