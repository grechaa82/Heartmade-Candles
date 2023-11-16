using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class ConfiguredCandleBasketRequest
{
    [Required, JsonPropertyName("candleDetailFilterRequests")]
    public required CandleDetailFilterRequest[] CandleDetailFilterRequests { get; set; }

    [Required, JsonPropertyName("configuredCandleFiltersString")]
    public required string ConfiguredCandleFiltersString { get; set; }
}
