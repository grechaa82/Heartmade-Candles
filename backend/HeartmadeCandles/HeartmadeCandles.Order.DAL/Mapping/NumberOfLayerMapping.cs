using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class NumberOfLayerMapping
{
    public static NumberOfLayer MapToNumberOfLayer(NumberOfLayerCollection numberOfLayerCollection)
    {
        return new NumberOfLayer(
            numberOfLayerCollection.Id,
            numberOfLayerCollection.Number
        );
    }

    public static NumberOfLayerCollection MapToNumberOfLayerCollection(NumberOfLayer numberOfLayer)
    {
        return new NumberOfLayerCollection
        {
            Id = numberOfLayer.Id,
            Number = numberOfLayer.Number
        };
    }
}
