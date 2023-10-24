using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class CreateOrderRequest
{
    [Required]
    public required string ConfiguredCandlesString { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public required UserRequest User { get; set; }

    [Required]
    public required FeedbackRequest Feedback { get; set; }
}