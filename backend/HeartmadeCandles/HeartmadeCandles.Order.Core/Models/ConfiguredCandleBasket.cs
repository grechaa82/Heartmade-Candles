namespace HeartmadeCandles.Order.Core.Models;

public class ConfiguredCandleBasket
{
    public required ConfiguredCandleFilter[] ConfiguredCandleFilters { get; set; }

    public required string ConfiguredCandleFiltersString { get; set; }
}
