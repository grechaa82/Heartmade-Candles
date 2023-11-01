using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class OrderNumberOfLayerRequest
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public int Number { get; set; }
}

