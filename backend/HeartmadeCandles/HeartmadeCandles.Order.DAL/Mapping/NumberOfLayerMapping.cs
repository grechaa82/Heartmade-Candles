using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class NumberOfLayerMapping
{
    public static NumberOfLayer MapToNumberOfLayer(NumberOfLayerDocument numberOfLayerDocument)
    {
        return new NumberOfLayer(
            numberOfLayerDocument.Id,
            numberOfLayerDocument.Number
        );
    }

    public static NumberOfLayerDocument MapToNumberOfLayerDocument(NumberOfLayer numberOfLayer)
    {
        return new NumberOfLayerDocument
        {
            Id = numberOfLayer.Id,
            Number = numberOfLayer.Number
        };
    }
}
