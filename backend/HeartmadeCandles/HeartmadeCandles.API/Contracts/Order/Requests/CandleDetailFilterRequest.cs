using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class CandleDetailFilterRequest
{
    [Required]
    public int CandleId { get; set; }

    public int? DecorId { get; set; }

    [Required]
    public int NumberOfLayerId { get; set; }

    [Required]
    public required int[] LayerColorIds { get; set; }

    public int? SmellId { get; set; }

    [Required]
    public int WickId { get; set; }

    [Required]
    public int Quantity { get; set; }

    public string? FilterString { get; set; }
}