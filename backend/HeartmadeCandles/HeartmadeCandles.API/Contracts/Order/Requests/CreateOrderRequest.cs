using System.ComponentModel.DataAnnotations;
using HeartmadeCandles.API.Contracts.Requests;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class CreateOrderRequest
{
    [Required]
    public required string ConfiguredCandlesString { get; set; }

    [Required]
    public required string BasketId { get; set; }

    public UserRequest? User { get; set; }

    public FeedbackRequest? Feedback { get; set; }
}