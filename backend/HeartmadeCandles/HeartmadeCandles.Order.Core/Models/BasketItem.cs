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
        var result = Result.Success();

        if (ConfiguredCandle.Candle.Id != ConfiguredCandleFilter.CandleId)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Candle by id: {ConfiguredCandleFilter.CandleId} does not match with candle by id: {ConfiguredCandle.Candle.Id}"));
        }

        if (ConfiguredCandleFilter.DecorId != 0)
        {
            if (ConfiguredCandle.Decor == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Decor by id: {ConfiguredCandleFilter.DecorId} is not found"));
            }
            else if (ConfiguredCandle.Decor.Id != ConfiguredCandleFilter.DecorId)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Decor by id: {ConfiguredCandleFilter.DecorId} does not match with decor by id: {ConfiguredCandle.Decor.Id}"));
            }
        }
        else if (ConfiguredCandle.Decor != null)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Decor by id: {ConfiguredCandleFilter.DecorId} is found, but it should not be in"));
        }

        if (ConfiguredCandle.NumberOfLayer.Id != ConfiguredCandleFilter.NumberOfLayerId)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"NumberOfLayer by id: {ConfiguredCandleFilter.NumberOfLayerId} does not match with numberOfLayer by id: {ConfiguredCandle.NumberOfLayer.Id}"));
        }

        if (ConfiguredCandle.LayerColors.Any() && ConfiguredCandle.NumberOfLayer.Number != ConfiguredCandle.LayerColors.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Number of layers '{ConfiguredCandle.NumberOfLayer.Number}' does not match the actual number '{ConfiguredCandle.LayerColors.Length}'"));
        }

        if (!ConfiguredCandle.LayerColors.Any())
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    "LayerColors cannot be null or empty"));
        }

        if (ConfiguredCandle.LayerColors.Length != ConfiguredCandleFilter.LayerColorIds.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    "Length of LayerColorIds is incorrect"));
        }
        else
        {
            for (var i = 0; i < ConfiguredCandleFilter.LayerColorIds.Length; i++)
                if (ConfiguredCandleFilter.LayerColorIds[i] != ConfiguredCandle.LayerColors[i].Id)
                {
                    result = Result.Combine(
                        result,
                        Result.Failure(
                            $"LayerColor by id: {ConfiguredCandleFilter.LayerColorIds[i]} does not match with layerColor by id: {ConfiguredCandle.LayerColors[i].Id}"));
                }
        }

        if (ConfiguredCandle.Wick.Id != ConfiguredCandleFilter.WickId)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Wick by id: {ConfiguredCandleFilter.WickId} does not match with wick by id: {ConfiguredCandle.Wick.Id}"));
        }

        if (ConfiguredCandleFilter.SmellId != 0)
        {
            if (ConfiguredCandle.Smell == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Smell by id: {ConfiguredCandleFilter.SmellId} is not found"));
            }
            else if (ConfiguredCandle.Smell.Id != ConfiguredCandleFilter.SmellId)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Smell by id: {ConfiguredCandleFilter.SmellId} does not match with smell by id: {ConfiguredCandle.Smell.Id}"));
            }
        }
        else if (ConfiguredCandle.Smell != null)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Smell by id: {ConfiguredCandleFilter.SmellId} is found, but it should not be in"));
        }

        return result.IsFailure
            ? Result.Failure(result.Error)
            : Result.Success();
    }

    public Result IsComparedConfiguredCandles(ConfiguredCandle configuredCandle)
    {
        var result = Result.Success();

        if (ConfiguredCandle.Candle.Id != configuredCandle.Candle.Id)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Candle by id: {configuredCandle.Candle.Id} does not match with candle by id: {ConfiguredCandle.Candle.Id}"));
        }

        if (configuredCandle.Decor != null && configuredCandle.Decor.Id != 0)
        {
            if (ConfiguredCandle.Decor == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Decor by id: {configuredCandle.Decor?.Id} is not found"));
            }
            else if (ConfiguredCandle.Decor.Id != configuredCandle.Decor?.Id)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Decor by id: {configuredCandle.Decor?.Id} does not match with decor by id: {ConfiguredCandle.Decor.Id}"));
            }
        }
        else if (ConfiguredCandle.Decor != null)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Decor by id: {configuredCandle.Decor.Id} is found, but it should not be in"));
        }

        if (ConfiguredCandle.NumberOfLayer.Id != configuredCandle.NumberOfLayer.Id)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"NumberOfLayer by id: {configuredCandle.NumberOfLayer.Id} does not match with numberOfLayer by id: {ConfiguredCandle.NumberOfLayer.Id}"));
        }

        if (ConfiguredCandle.LayerColors.Any() && ConfiguredCandle.NumberOfLayer.Number != ConfiguredCandle.LayerColors.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Number of layers '{ConfiguredCandle.NumberOfLayer.Number}' does not match the actual number '{ConfiguredCandle.LayerColors.Length}'"));
        }

        if (!ConfiguredCandle.LayerColors.Any())
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    "LayerColors cannot be null or empty"));
        }

        if (ConfiguredCandle.LayerColors.Length != configuredCandle.LayerColors.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    "Length of LayerColorIds is incorrect"));
        }
        else
        {
            for (var i = 0; i < configuredCandle.LayerColors.Length; i++)
                if (configuredCandle.LayerColors[i].Id != ConfiguredCandle.LayerColors[i].Id)
                {
                    result = Result.Combine(
                        result,
                        Result.Failure(
                            $"LayerColor by id: {configuredCandle.LayerColors[i].Id} does not match with layerColor by id: {ConfiguredCandle.LayerColors[i].Id}"));
                }
        }

        if (ConfiguredCandle.Wick.Id != configuredCandle.Wick.Id)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Wick by id: {configuredCandle.Wick.Id} does not match with wick by id: {ConfiguredCandle.Wick.Id}"));
        }

        if (configuredCandle.Smell != null && configuredCandle.Smell.Id != 0)
        {
            if (ConfiguredCandle.Smell == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure($"Smell by id: {configuredCandle.Smell?.Id} is not found"));
            }
            else if (ConfiguredCandle.Smell.Id != configuredCandle.Smell?.Id)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Smell by id: {configuredCandle.Smell?.Id} does not match with smell by id: {ConfiguredCandle.Smell.Id}"));
            }
        }
        else if (ConfiguredCandle.Smell != null)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Smell by id: {configuredCandle.Smell?.Id} is found, but it should not be in"));
        }

        return result.IsFailure
            ? Result.Failure(result.Error)
            : Result.Success();
    }
}