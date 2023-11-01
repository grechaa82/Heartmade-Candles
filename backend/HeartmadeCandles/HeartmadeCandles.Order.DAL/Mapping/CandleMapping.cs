using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class CandleMapping
{
    public static Candle MapToCandle(CandleCollection candleCollection)
    {
        return new Candle(
            candleCollection.Id,
            candleCollection.Title,
            candleCollection.Price,
            candleCollection.WeightGrams,
            ImageMapping.MapToImages(candleCollection.Images),
            TypeCandleMapping.MapToTypeCandle(candleCollection.TypeCandle)
        );
    }

    public static CandleCollection MapToCandleCollection(Candle candle)
    {
        return new CandleCollection
        {
            Id = candle.Id,
            Title = candle.Title,
            Price = candle.Price,
            WeightGrams = candle.WeightGrams,
            Images = ImageMapping.MapToImagesCollection(candle.Images),
            TypeCandle = TypeCandleMapping.MapToTypeCandleCollection(candle.TypeCandle)
        };
    }
}

