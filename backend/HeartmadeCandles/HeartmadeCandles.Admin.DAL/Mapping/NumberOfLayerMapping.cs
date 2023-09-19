using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping;

internal class NumberOfLayerMapping
{
    public static NumberOfLayer MapToNumberOfLayer(NumberOfLayerEntity numberOfLayerEntity)
    {
        var numberOfLayer = NumberOfLayer.Create(numberOfLayerEntity.Number, numberOfLayerEntity.Id);

        return numberOfLayer.Value;
    }

    public static NumberOfLayerEntity MapToNumberOfLayerEntity(NumberOfLayer numberOfLayer)
    {
        var numberOfLayerEntity = new NumberOfLayerEntity
        {
            Id = numberOfLayer.Id,
            Number = numberOfLayer.Number
        };

        return numberOfLayerEntity;
    }
}