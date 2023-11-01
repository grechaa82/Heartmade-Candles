using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class OrderDetailItemRequest
{
    [Required]
    public required OrderCandleRequest Candle { get; set; }

    public OrderDecorRequest? Decor { get; set; }

    [Required]
    public required OrderLayerColorRequest[] LayerColors { get; set; }

    [Required]
    public required OrderNumberOfLayerRequest NumberOfLayer { get; set; }

    public OrderSmellRequest? Smell { get; set; }

    [Required]
    public required OrderWickRequest Wick { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public required string ConfigurationString { get; set; }
}
