using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class ConfiguredCandleFilterMapping
{
    public static ConfiguredCandleFilter MapToConfiguredCandleFilter(ConfiguredCandleFilterDocument configuredCandleFilterDocument)
    {
        return new ConfiguredCandleFilter
        {
            CandleId = configuredCandleFilterDocument.CandleId,
            DecorId = configuredCandleFilterDocument.DecorId,
            NumberOfLayerId = configuredCandleFilterDocument.NumberOfLayerId,
            LayerColorIds = configuredCandleFilterDocument.LayerColorIds,
            SmellId = configuredCandleFilterDocument.SmellId,
            WickId = configuredCandleFilterDocument.WickId,
            Quantity = configuredCandleFilterDocument.Quantity 
        };
    }

    public static ConfiguredCandleFilterDocument MapToConfiguredCandleFilterDocument(ConfiguredCandleFilter configuredCandleFilter)
    {
        return new ConfiguredCandleFilterDocument
        {
            CandleId = configuredCandleFilter.CandleId,
            DecorId = configuredCandleFilter.DecorId,
            NumberOfLayerId = configuredCandleFilter.NumberOfLayerId,
            LayerColorIds = configuredCandleFilter.LayerColorIds,
            SmellId = configuredCandleFilter.SmellId,
            WickId = configuredCandleFilter.WickId,
            Quantity = configuredCandleFilter.Quantity,
        };
    }
}
