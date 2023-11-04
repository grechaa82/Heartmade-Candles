using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class TypeCandleMapping
{
    public static TypeCandle MapToTypeCandle(TypeCandleDocument typeCandleDocument)
    {
        return new TypeCandle(
            typeCandleDocument.Id,
            typeCandleDocument.Title
        );
    }

    public static TypeCandleDocument MapToTypeCandleDocument(TypeCandle typeCandle)
    {
        return new TypeCandleDocument
        {
            Id = typeCandle.Id,
            Title = typeCandle.Title
        };
    }
}
 
