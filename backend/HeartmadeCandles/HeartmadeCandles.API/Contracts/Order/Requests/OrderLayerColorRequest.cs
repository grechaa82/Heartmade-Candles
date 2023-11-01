using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class OrderLayerColorRequest
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public decimal PricePerGram { get; set; }
}

