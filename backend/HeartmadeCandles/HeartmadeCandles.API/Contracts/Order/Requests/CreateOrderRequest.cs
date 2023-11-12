using System.ComponentModel.DataAnnotations;
using HeartmadeCandles.API.Contracts.Requests;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class CreateOrderRequest
{
    [Required]
    public required string ConfiguredCandlesString { get; set; }

    [Required]
    public required string BasketId { get; set; }

    [Required]
    public required UserRequest User { get; set; }

    [Required]
    public required FeedbackRequest Feedback { get; set; }
}