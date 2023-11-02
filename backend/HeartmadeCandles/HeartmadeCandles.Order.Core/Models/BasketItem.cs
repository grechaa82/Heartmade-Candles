using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models;

public class BasketItem
{
    public required ConfiguredCandle ConfiguredCandle { get; set; }

    public decimal Price { get; set; }

    public int Quantity => ConfiguredCandleFilter.Quantity;

    public required ConfiguredCandleFilter ConfiguredCandleFilter { get; set; }

    public Result CheckConsistencyConfiguredCandle()
    {
        if (ConfiguredCandle.Candle.Id != ConfiguredCandleFilter.CandleId)
        {
            return Result.Failure("");
        }
        if (ConfiguredCandle.Decor?.Id != ConfiguredCandleFilter.DecorId)
        {
            return Result.Failure("");
        }
        if (ConfiguredCandle.NumberOfLayer.Id != ConfiguredCandleFilter.NumberOfLayerId)
        {
            return Result.Failure("");
        }

        throw new NotImplementedException();
    }
}