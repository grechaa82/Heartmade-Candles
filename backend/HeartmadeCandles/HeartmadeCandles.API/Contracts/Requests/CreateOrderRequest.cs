using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class CreateOrderRequest
{
    [Required] public string ConfiguredCandlesString { get; set; }

    [Required] public OrderItemFilterRequest[] OrderItemFilters { get; set; }

    [Required] public UserRequest User { get; set; }

    [Required] public FeedbackRequest Feedback { get; set; }
}