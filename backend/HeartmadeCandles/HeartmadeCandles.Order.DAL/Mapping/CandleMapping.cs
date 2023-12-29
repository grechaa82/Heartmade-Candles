using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class CandleMapping
{
    public static Candle MapToCandle(CandleDocument candleDocument)
    {
        return new Candle
        {
            Id = candleDocument.Id,
            Title = candleDocument.Title,
            Price = candleDocument.Price,
            WeightGrams = candleDocument.WeightGrams,
            Images = ImageMapping.MapToImages(candleDocument.Images)
        };
    }

    public static CandleDocument MapToCandleDocument(Candle candle)
    {
        return new CandleDocument
        {
            Id = candle.Id,
            Title = candle.Title,
            Price = candle.Price,
            WeightGrams = candle.WeightGrams,
            Images = ImageMapping.MapToImagesDocument(candle.Images)
        };
    }
}

