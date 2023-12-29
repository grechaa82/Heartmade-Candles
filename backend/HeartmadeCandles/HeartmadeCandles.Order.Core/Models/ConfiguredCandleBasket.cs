namespace HeartmadeCandles.Order.Core.Models;

public class ConfiguredCandleBasket
{
    public required ConfiguredCandleFilter[] ConfiguredCandleFilters { get; init; }

    public required string ConfiguredCandleFiltersString { get; init; }
}
