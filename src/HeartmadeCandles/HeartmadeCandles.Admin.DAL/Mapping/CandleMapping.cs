using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping
{
    internal class CandleMapping
    {
        public static Candle MapToCandle(CandleEntity candleEntity, TypeCandle typeCandle)
        {
            var candle = Candle.Create(
                candleEntity.Title,
                candleEntity.Description,
                candleEntity.Price,
                candleEntity.WeightGrams,
                candleEntity.ImageURL,
                typeCandle,
                candleEntity.IsActive,
                candleEntity.Id,
                candleEntity.CreatedAt);

            return candle.Value;
        }

        public static CandleEntity MapToCandleEntity(Candle candle)
        {
            var candleEntity = new CandleEntity()
            {
                Id = candle.Id,
                Title = candle.Title,
                Description = candle.Description,
                Price = candle.Price,
                WeightGrams = candle.WeightGrams,
                ImageURL = candle.ImageURL,
                IsActive = candle.IsActive,
                TypeCandleId = candle.TypeCandle.Id,
                CreatedAt = candle.CreatedAt
            };

            return candleEntity;
        }
    }
}
