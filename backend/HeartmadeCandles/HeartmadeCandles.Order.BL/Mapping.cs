using HeartmadeCandles.Constructor.Core.Models;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL;

internal class Mapping
{
    public static ConfiguredCandle MapConstructorCandleDetailToOrderConfiguredCandle(Constructor.Core.Models.CandleDetail candleDetail)
    {
        return new ConfiguredCandle
        {
            Candle = MapConstructorCandleToOrderCandle(candleDetail.Candle),
            Decor = candleDetail.Decors.FirstOrDefault() != null
                ? MapConstructorDecorToOrderDecor(candleDetail.Decors[0])
                : null,
            LayerColors = MapConstructorLayerColorsToOrderLayerColors(candleDetail.LayerColors),
            NumberOfLayer = candleDetail.NumberOfLayers.Any() 
                ? MapConstructorNumberOfLayerToOrderNumberOfLayer(candleDetail.NumberOfLayers[0]) 
                : throw new InvalidCastException(),
            Smell = candleDetail.Smells.FirstOrDefault() != null
                ? MapConstructorSmellToOrderSmell(candleDetail.Smells[0])
                : null,
            Wick = candleDetail.Wicks.Any() 
                ? MapConstructorWickToOrderWick(candleDetail.Wicks[0]) 
                : throw new InvalidCastException()
        };
    }

    public static Core.Models.Candle MapConstructorCandleToOrderCandle(Constructor.Core.Models.Candle candle)
    {
        return new Core.Models.Candle(
            candle.Id,
            candle.Title,
            candle.Price,
            candle.WeightGrams,
            MapConstructorImageToOrderImage(candle.Images));
    }

    public static Core.Models.Decor MapConstructorDecorToOrderDecor(Constructor.Core.Models.Decor decor)
    {
        return new Core.Models.Decor(decor.Id, decor.Title, decor.Price);
    }

    public static Core.Models.LayerColor[] MapConstructorLayerColorsToOrderLayerColors(Constructor.Core.Models.LayerColor[] layerColor)
    {
        return layerColor
            .Select(x => new Core.Models.LayerColor(x.Id, x.Title, x.PricePerGram))
            .ToArray();
    }

    public static Core.Models.NumberOfLayer MapConstructorNumberOfLayerToOrderNumberOfLayer(Constructor.Core.Models.NumberOfLayer numberOfLayer)
    {
        return new Core.Models.NumberOfLayer(numberOfLayer.Id, numberOfLayer.Number);
    }

    public static Core.Models.Smell MapConstructorSmellToOrderSmell(Constructor.Core.Models.Smell smell)
    {
        return new Core.Models.Smell(smell.Id, smell.Title, smell.Price);
    }

    public static Core.Models.Wick MapConstructorWickToOrderWick(Constructor.Core.Models.Wick wick)
    {
        return new Core.Models.Wick(wick.Id, wick.Title, wick.Price);
    }

    public static Core.Models.Image[] MapConstructorImageToOrderImage(Constructor.Core.Models.Image[] image)
    {
        return image
            .Select(x => new Core.Models.Image(x.FileName, x.AlternativeName))
            .ToArray();
    }

    public static CandleDetailFilter MapToCandleDetailFilter(ConfiguredCandleFilter filter)
    {
        return new CandleDetailFilter
        {
            CandleId = filter.CandleId,
            DecorId = filter.DecorId,
            NumberOfLayerId = filter.NumberOfLayerId,
            LayerColorIds = filter.LayerColorIds,
            SmellId = filter.SmellId,
            WickId = filter.WickId
        };
    }
}
