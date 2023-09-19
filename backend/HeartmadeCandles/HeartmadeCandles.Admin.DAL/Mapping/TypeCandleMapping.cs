using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping;

internal class TypeCandleMapping
{
    public static TypeCandle MapToCandleType(TypeCandleEntity typeCandleEntity)
    {
        var typeCandle = TypeCandle.Create(typeCandleEntity.Title, typeCandleEntity.Id);

        return typeCandle.Value;
    }

    public static TypeCandleEntity MapToTypeCandleEntity(TypeCandle typeCandle)
    {
        var typeCandleEntity = new TypeCandleEntity
        {
            Id = typeCandle.Id,
            Title = typeCandle.Title
        };

        return typeCandleEntity;
    }
}