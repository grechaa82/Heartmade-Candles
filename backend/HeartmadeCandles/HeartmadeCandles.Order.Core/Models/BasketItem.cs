using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;

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
        // Результом будет один Result.Failure (Вот этот почти всегда "The configured layer colors and their count do not match the specified criteria")
        var resultV1 = Result.Success()
            .BindIf(price <= 0, () => Result.Failure($"{nameof(Price)} cannot be 0 or less"))
            .Bind(() => CheckIdMismatch(
                expectedId: configuredCandleFilter.CandleId,
                actualId: configuredCandle.Candle.Id,
                entityName: nameof(configuredCandle.Candle)))
            .BindIf(configuredCandleFilter.DecorId != 0 && configuredCandle.Decor != null,
                () => CheckIdMismatch(
                    expectedId: configuredCandleFilter.DecorId,
                    actualId: configuredCandle.Decor?.Id,
                    entityName: nameof(configuredCandle.Decor)))
            .Bind(() => CheckEntityExistenceAndMismatch(
                expectedEntity: configuredCandleFilter.DecorId,
                actualEntity: configuredCandle.Decor?.Id,
                entityName: nameof(configuredCandle.Decor)))
            .Bind(() => CheckIdMismatch(
                expectedId: configuredCandleFilter.NumberOfLayerId,
                actualId: configuredCandle.NumberOfLayer.Id,
                entityName: nameof(configuredCandle.NumberOfLayer)))
            .Bind(() => CheckLayerColorCountMatchAndNotEmpty(
                layerColors: configuredCandle.LayerColors,
                expectedCount: configuredCandle.NumberOfLayer.Number))
            .Bind(() => CheckLayerColorsIdsMatch(
                expectedLayerColorIds: configuredCandleFilter.LayerColorIds,
                actualLayerColorIds: configuredCandle.LayerColors.Select(lc => lc.Id).ToArray()))
            .BindIf(configuredCandle.LayerColors.Length == configuredCandle.NumberOfLayer.Number
                    && configuredCandle.NumberOfLayer.Number == configuredCandleFilter.LayerColorIds.Length,
                () => Result.Failure($"The configured layer colors and their count do not match the specified criteria"))
            .BindIf(configuredCandleFilter.SmellId != 0 && configuredCandle.Smell != null,
                () => CheckIdMismatch(
                    expectedId: configuredCandleFilter.SmellId,
                    actualId: configuredCandle.Smell?.Id,
                    entityName: nameof(configuredCandle.Smell)))
            .Bind(() => CheckEntityExistenceAndMismatch(
                expectedEntity: configuredCandleFilter.SmellId,
                actualEntity: configuredCandle.Smell?.Id,
                entityName: nameof(configuredCandle.Smell)))
            .Bind(() => CheckIdMismatch(
                expectedId: configuredCandleFilter.WickId,
                actualId: configuredCandle.Wick.Id,
                entityName: nameof(configuredCandle.Wick)))
            .Map(() => new BasketItem(configuredCandle, price, configuredCandleFilter))
            .TapError(error => Result.Failure<BasketItem>(error));

        // Результом с комбинированным Result.Failure
        var resultV2 = Result.Combine(
            Result.FailureIf(price <= 0, $"{nameof(Price)} cannot be 0 or less"),
            CheckIdMismatch(
                expectedId: configuredCandleFilter.CandleId,
                actualId: configuredCandle.Candle.Id,
                entityName: nameof(configuredCandle.Candle)),
            configuredCandleFilter.DecorId != 0 && configuredCandle.Decor != null 
                ? CheckIdMismatch(
                    expectedId: configuredCandleFilter.DecorId,
                    actualId: configuredCandle.Decor?.Id,
                    entityName: nameof(configuredCandle.Decor)) 
                : Result.Success(),
            CheckEntityExistenceAndMismatch(
                expectedEntity: configuredCandleFilter.DecorId,
                actualEntity: configuredCandle.Decor?.Id,
                entityName: nameof(configuredCandle.Decor)),
            CheckIdMismatch(
                expectedId: configuredCandleFilter.NumberOfLayerId,
                actualId: configuredCandle.NumberOfLayer.Id,
                entityName: nameof(configuredCandle.NumberOfLayer)),
            CheckLayerColorCountMatchAndNotEmpty(
                layerColors: configuredCandle.LayerColors,
                expectedCount: configuredCandle.NumberOfLayer.Number),
            CheckLayerColorsIdsMatch(
                expectedLayerColorIds: configuredCandleFilter.LayerColorIds,
                actualLayerColorIds: configuredCandle.LayerColors.Select(lc => lc.Id).ToArray()),
            configuredCandle.LayerColors.Length == configuredCandle.NumberOfLayer.Number
                && configuredCandle.NumberOfLayer.Number == configuredCandleFilter.LayerColorIds.Length 
                    ? Result.Success() 
                    : Result.Failure("The configured layer colors and their count do not match the specified criteria"),
            configuredCandleFilter.SmellId != 0 && configuredCandle.Smell != null 
                ? CheckIdMismatch(
                    expectedId: configuredCandleFilter.SmellId,
                    actualId: configuredCandle.Smell?.Id,
                    entityName: nameof(configuredCandle.Smell)) 
                : Result.Success(),
            CheckEntityExistenceAndMismatch(
                expectedEntity: configuredCandleFilter.SmellId,
                actualEntity: configuredCandle.Smell?.Id,
                entityName: nameof(configuredCandle.Smell)),
            CheckIdMismatch(
                expectedId: configuredCandleFilter.WickId,
                actualId: configuredCandle.Wick.Id,
                entityName: nameof(configuredCandle.Wick))
        )
        .Map(() => new BasketItem(configuredCandle, price, configuredCandleFilter));

        return resultV2;
    }

    public Result Compare(ConfiguredCandle configuredCandle)
    {
        return Result.Success()
            .Bind(() => CheckIdMismatch(
                expectedId: ConfiguredCandle.Candle.Id,
                actualId: configuredCandle.Candle.Id,
                entityName: nameof(ConfiguredCandle.Candle)))
            .Bind(() => CheckPriceMismatch(
                expectedPrice: ConfiguredCandle.Candle.Price,
                actualPrice: configuredCandle.Candle.Price,
                entityName: nameof(ConfiguredCandle.Candle)))
            .Bind(() => CheckIdMismatch(
                expectedId: configuredCandle.NumberOfLayer.Id,
                actualId: ConfiguredCandle.NumberOfLayer.Id,
                entityName: nameof(ConfiguredCandle.NumberOfLayer)))
            .Bind(() => CheckLayerColorCountMatchAndNotEmpty(
                layerColors: ConfiguredCandle.LayerColors,
                expectedCount: configuredCandle.NumberOfLayer.Number))
            .Bind(() => CheckIdMismatch(
                expectedId: ConfiguredCandle.Decor?.Id,
                actualId: configuredCandle.Decor?.Id,
                entityName: nameof(ConfiguredCandle.Decor)))
            .Bind(() => CheckPriceMismatch(
                expectedPrice: ConfiguredCandle.Decor?.Price,
                actualPrice: configuredCandle.Decor?.Price,
                entityName: nameof(ConfiguredCandle.Decor)))
            .Bind(() => CheckEntityExistenceAndMismatch(
                expectedEntity: ConfiguredCandle.Decor,
                actualEntity: configuredCandle.Decor,
                entityName: nameof(ConfiguredCandle.Decor)))
            .Bind(() => CheckLayerColorsIdsMatch(
                expectedLayerColorIds: ConfiguredCandle.LayerColors.Select(lc => lc.Id).ToArray(),
                actualLayerColorIds: configuredCandle.LayerColors.Select(lc => lc.Id).ToArray()))
            .Bind(() => CheckIdMismatch(
                expectedId: ConfiguredCandle.Smell?.Id,
                actualId: configuredCandle.Smell?.Id,
                entityName: nameof(ConfiguredCandle.Smell)))
            .Bind(() => CheckPriceMismatch(
                expectedPrice: ConfiguredCandle.Smell?.Price,
                actualPrice: configuredCandle.Smell?.Price,
                entityName: nameof(ConfiguredCandle.Smell)))
            .Bind(() => CheckIdMismatch(
                expectedId: ConfiguredCandle.Wick.Id,
                actualId: configuredCandle.Wick.Id,
                entityName: nameof(ConfiguredCandle.Wick)))
            .Bind(() => CheckPriceMismatch(
                expectedPrice: ConfiguredCandle.Wick.Price,
                actualPrice: configuredCandle.Wick.Price,
                entityName: nameof(ConfiguredCandle.Wick)));
    }

    private static Result CheckIdMismatch(int? expectedId, int? actualId, string entityName)
    {
        return expectedId != actualId
            ? Result.Failure($"{entityName} by id: {expectedId} does not match with {entityName} by id: {actualId}")
            : Result.Success();
    }

    private static Result CheckPriceMismatch(decimal? expectedPrice, decimal? actualPrice, string entityName)
    {
        return expectedPrice.HasValue && actualPrice.HasValue && expectedPrice != actualPrice
            ? Result.Failure($"Prices of {entityName} do not match")
            : Result.Success();
    }

    private static Result CheckEntityExistenceAndMismatch<T>(T? expectedEntity, T? actualEntity, string entityName) where T : class
    {
        if (expectedEntity != null && actualEntity == null)
        {
            return Result.Failure($"{entityName} does not match the expected");
        }
        else if (expectedEntity == null && actualEntity != null)
        {
            return Result.Failure($"{entityName} is found, but it should not be in");
        }

        return Result.Success();
    }

    private static Result CheckEntityExistenceAndMismatch(int? expectedEntity, int? actualEntity, string entityName)
    {
        if (expectedEntity == 0 && actualEntity != null)
        {
            return Result.Failure($"{entityName} by id: {actualEntity} is found, but it should not be in");
        }
        if ((expectedEntity == 0 || expectedEntity == null) && actualEntity != null)
        {
            return Result.Failure($"{entityName} by id: {expectedEntity} is not found");
        }
        else if (expectedEntity == null && actualEntity != null)
        {
            return Result.Failure($"{entityName} is found, but it should not be in");
        }

        return Result.Success();
    }

    private static Result CheckLayerColorCountMatchAndNotEmpty(LayerColor[] layerColors, int expectedCount)
    {
        if (!layerColors.Any())
        {
            return Result.Failure("LayerColors cannot be null or empty");
        }

        if (layerColors.Length != expectedCount)
        {
            return Result.Failure($"Number of layers '{expectedCount}' does not match the actual number '{layerColors.Length}'");
        }

        return Result.Success();
    }

    private static Result CheckLayerColorsIdsMatch(int[] expectedLayerColorIds, int[] actualLayerColorIds)
    {
        if (expectedLayerColorIds.Length <= 0 || actualLayerColorIds.Length <= 0 || expectedLayerColorIds.Length != actualLayerColorIds.Length)
        {
            return Result.Success();
        }
        for (var i = 0; i < expectedLayerColorIds.Length; i++)
        {
            if (expectedLayerColorIds[i] != actualLayerColorIds[i])
            {
                return Result.Failure($"LayerColors by id: {expectedLayerColorIds[i]} does not match with LayerColors by id: {actualLayerColorIds[i]}");
            }
        }

        return Result.Success();
    }
}