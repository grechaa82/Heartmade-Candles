using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class ConfiguredCandleBasketRequest
{
    [Required]
    public required CandleDetailFilterRequest[] CandleDetailFilterRequests { get; set; }

    [Required]
    public required string ConfiguredCandleFiltersString { get; set; }
}
