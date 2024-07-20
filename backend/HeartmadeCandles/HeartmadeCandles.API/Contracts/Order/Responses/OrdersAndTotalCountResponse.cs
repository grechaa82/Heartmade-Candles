using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Responses;

public class OrdersAndTotalCountResponse
{
    [Required]
    public required HeartmadeCandles.Order.Core.Models.Order[] Orders { get; set; }

    [Required]
    public long TotalCount { get; set; }
}
