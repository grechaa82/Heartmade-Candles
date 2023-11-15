using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Core.Models;

public class BasketItem
{
    private BasketItem(
        ConfiguredCandle configuredCandle,
        decimal price,
        ConfiguredCandleFilter configuredCandleFilter)
    {
        ConfiguredCandle = configuredCandle;
        Price = price;
        ConfiguredCandleFilter = configuredCandleFilter;
    }

    public ConfiguredCandle ConfiguredCandle { get; }

    public decimal Price { get; }

    public int Quantity => ConfiguredCandleFilter.Quantity;

    public ConfiguredCandleFilter ConfiguredCandleFilter { get; }

    public static Result<BasketItem> Create(
        ConfiguredCandle configuredCandle, 
        decimal price, 
        ConfiguredCandleFilter configuredCandleFilter)
    {
        var result = Result.Success();

        if (price <= 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<BasketItem>($"'{nameof(price)}' cannot be 0 or less"));
        }

        if (configuredCandle.Candle.Id != configuredCandleFilter.CandleId)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Candle by id: {configuredCandleFilter.CandleId} does not match with candle by id: {configuredCandle.Candle.Id}"));
        }

        if (configuredCandleFilter.DecorId != 0)
        {
            if (configuredCandle.Decor == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Decor by id: {configuredCandleFilter.DecorId} is not found"));
            }
            else if (configuredCandle.Decor.Id != configuredCandleFilter.DecorId)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Decor by id: {configuredCandleFilter.DecorId} does not match with decor by id: {configuredCandle.Decor.Id}"));
            }
        }
        else
        {
            if (configuredCandle.Decor != null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Decor by id: {configuredCandle.Decor.Id} is found, but it should not be in"));
            }
        }


        if (configuredCandle.NumberOfLayer.Id != configuredCandleFilter.NumberOfLayerId)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"NumberOfLayer by id: {configuredCandleFilter.NumberOfLayerId} does not match with numberOfLayer by id: {configuredCandle.NumberOfLayer.Id}"));
        }

        if (configuredCandle.LayerColors.Any() && configuredCandle.NumberOfLayer.Number != configuredCandle.LayerColors.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Number of layers '{configuredCandle.NumberOfLayer.Number}' does not match the actual number '{configuredCandle.LayerColors.Length}'"));
        }

        if (!configuredCandle.LayerColors.Any())
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"{nameof(configuredCandle.LayerColors)} cannot be null or empty"));
        }

        if (configuredCandle.LayerColors.Length != configuredCandleFilter.LayerColorIds.Length)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Length of {nameof(configuredCandleFilter.LayerColorIds)} is incorrect"));
        }
        else
        {
            for (var i = 0; i < configuredCandleFilter.LayerColorIds.Length; i++)
                if (configuredCandleFilter.LayerColorIds[i] != configuredCandle.LayerColors[i].Id)
                {
                    result = Result.Combine(
                        result,
                        Result.Failure(
                            $"LayerColor by id: {configuredCandleFilter.LayerColorIds[i]} does not match with layerColor by id: {configuredCandle.LayerColors[i].Id}"));
                }
        }

        if (configuredCandle.Wick.Id != configuredCandleFilter.WickId)
        {
            result = Result.Combine(
                result,
                Result.Failure(
                    $"Wick by id: {configuredCandleFilter.WickId} does not match with wick by id: {configuredCandle.Wick.Id}"));
        }

        if (configuredCandleFilter.SmellId != 0)
        {
            if (configuredCandle.Smell == null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Smell by id: {configuredCandleFilter.SmellId} is not found"));
            }
            else if (configuredCandle.Smell.Id != configuredCandleFilter.SmellId)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Smell by id: {configuredCandleFilter.SmellId} does not match with smell by id: {configuredCandle.Smell.Id}"));
            }
        }
        else
        {
            if (configuredCandle.Smell != null)
            {
                result = Result.Combine(
                    result,
                    Result.Failure(
                        $"Smell by id: {configuredCandle.Smell.Id} is found, but it should not be in"));
            }
        }

        if (result.IsFailure)
        {
            return Result.Failure<BasketItem>(result.Error);
        }

        var basketItem = new BasketItem(configuredCandle, price, configuredCandleFilter);

        return Result.Success(basketItem);
    }

    public Result Compare(ConfiguredCandle configuredCandle)
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
                    $"Decor by id: {configuredCandle.Decor?.Id} is found, but it should not be in"));
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