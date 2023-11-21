using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services;

public class CalculateService : ICalculateService
{
    public Result<decimal> CalculatePrice(ConfiguredCandle configuredCandle)
    {
        var decorPrice = configuredCandle.Decor?.Price ?? 0;
        var smellPrice = configuredCandle.Smell?.Price ?? 0;

        var price = configuredCandle.Candle.Price + decorPrice + smellPrice + configuredCandle.Wick.Price;

        var gramsInLayer = configuredCandle.Candle.WeightGrams / configuredCandle.NumberOfLayer.Number;

        var layerColorsPrice = configuredCandle.LayerColors.Sum(l => l.CalculatePriceForGrams(gramsInLayer));

        price += layerColorsPrice;

        return Result.Success(price);
    }
}
