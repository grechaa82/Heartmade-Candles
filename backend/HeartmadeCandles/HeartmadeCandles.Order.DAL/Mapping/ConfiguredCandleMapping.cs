using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class ConfiguredCandleMapping
{
    public static ConfiguredCandle MapToConfiguredCandle(ConfiguredCandleDocument configuredCandlesDocument)
    {
        return new ConfiguredCandle
        {
            Candle = CandleMapping.MapToCandle(configuredCandlesDocument.Candle),
            Decor = configuredCandlesDocument.Decor==null ? null : DecorMapping.MapToDecor(configuredCandlesDocument.Decor),
            LayerColors = LayerColorMapping.MapToLayerColors(configuredCandlesDocument.LayerColors),
            NumberOfLayer = NumberOfLayerMapping.MapToNumberOfLayer(configuredCandlesDocument.NumberOfLayer),
            Smell = configuredCandlesDocument.Smell == null ? null : SmellMapping.MapToSmell(configuredCandlesDocument.Smell),
            Wick = WickMapping.MapToWick(configuredCandlesDocument.Wick),
        };
    }

    public static ConfiguredCandleDocument MapToConfiguredCandleDocument(ConfiguredCandle configuredCandles)
    {
        return new ConfiguredCandleDocument
        {
            Candle = CandleMapping.MapToCandleDocument(configuredCandles.Candle),
            Decor = configuredCandles.Decor == null ? null : DecorMapping.MapToDecorDocument(configuredCandles.Decor),
            LayerColors = LayerColorMapping.MapToLayerColorsDocument(configuredCandles.LayerColors),
            NumberOfLayer = NumberOfLayerMapping.MapToNumberOfLayerDocument(configuredCandles.NumberOfLayer),
            Smell = configuredCandles.Smell == null ? null : SmellMapping.MapToSmellDocument(configuredCandles.Smell),
            Wick = WickMapping.MapToWickDocument(configuredCandles.Wick),
        };
    }
}
