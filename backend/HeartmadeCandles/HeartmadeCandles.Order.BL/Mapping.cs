using HeartmadeCandles.Constructor.Core.Models;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL;

public class OrderMapping
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
        return new Core.Models.Candle
        {
            Id = candle.Id,
            Title = candle.Title,
            Price = candle.Price,
            WeightGrams = candle.WeightGrams,
            Images = MapConstructorImageToOrderImage(candle.Images)
        };
    }

    public static Core.Models.Decor MapConstructorDecorToOrderDecor(Constructor.Core.Models.Decor decor)
    {
        return new Core.Models.Decor
        {
            Id = decor.Id, 
            Title = decor.Title,
            Price = decor.Price
        };
    }

    public static Core.Models.LayerColor[] MapConstructorLayerColorsToOrderLayerColors(Constructor.Core.Models.LayerColor[] layerColor)
    {
        return layerColor
            .Select(x => new Core.Models.LayerColor
                {
                    Id = x.Id, 
                    Title = x.Title,
                    PricePerGram = x.PricePerGram
                })
            .ToArray();
    }

    public static Core.Models.NumberOfLayer MapConstructorNumberOfLayerToOrderNumberOfLayer(Constructor.Core.Models.NumberOfLayer numberOfLayer)
    {
        return new Core.Models.NumberOfLayer
        {
            Id =numberOfLayer.Id,
            Number = numberOfLayer.Number
        };
    }

    public static Core.Models.Smell MapConstructorSmellToOrderSmell(Constructor.Core.Models.Smell smell)
    {
        return new Core.Models.Smell
        {
            Id = smell.Id, 
            Title = smell.Title, 
            Price = smell.Price
        };
    }

    public static Core.Models.Wick MapConstructorWickToOrderWick(Constructor.Core.Models.Wick wick)
    {
        return new Core.Models.Wick
        {
            Id = wick.Id, 
            Title = wick.Title, 
            Price = wick.Price
        };
    }

    public static Core.Models.Image[] MapConstructorImageToOrderImage(Constructor.Core.Models.Image[] image)
    {
        return image
            .Select(x => new Core.Models.Image
                {
                    FileName = x.FileName,
                    AlternativeName = x.AlternativeName
                })
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
