using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models;

public class BasketItem
{
    public required ConfiguredCandle ConfiguredCandle { get; set; }

    public decimal Price { get; set; }

    public int Quantity => ConfiguredCandleFilter.Quantity;

    public required ConfiguredCandleFilter ConfiguredCandleFilter { get; set; }

    public Result IsMatchingConfiguredCandle()
    {
        return Result.Success();

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

    public Result IsComparedConfiguredCandles(ConfiguredCandle configuredCandle)
    {
        return Result.Success();
        if (ConfiguredCandle.Candle.Id != configuredCandle.Candle.Id)
        {
            return Result.Failure("");
        }
        if (ConfiguredCandle.Candle.Price != configuredCandle.Candle.Price)
        {
            return Result.Failure("");
        }
        if (ConfiguredCandle.Decor?.Id != configuredCandle.Decor?.Id)
        {
            return Result.Failure("");
        }
        if (ConfiguredCandle.Decor?.Price != configuredCandle.Decor?.Price)
        {
            return Result.Failure("");
        }
        if (ConfiguredCandle.NumberOfLayer.Id != configuredCandle.NumberOfLayer.Id)
        {
            return Result.Failure("");
        }
        if (ConfiguredCandle.NumberOfLayer.Number != configuredCandle.NumberOfLayer.Number)
        {
            return Result.Failure("");
        }

        throw new NotImplementedException();
    }
}