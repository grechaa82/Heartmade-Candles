using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class TypeCandleMapping
{
    public static TypeCandle MapToTypeCandle(TypeCandleCollection typeCandleCollection)
    {
        return new TypeCandle(
            typeCandleCollection.Id,
            typeCandleCollection.Title
        );
    }

    public static TypeCandleCollection MapToTypeCandleCollection(TypeCandle typeCandle)
    {
        return new TypeCandleCollection
        {
            Id = typeCandle.Id,
            Title = typeCandle.Title
        };
    }
}
 
