using HeartmadeCandles.API.Contracts.Requests;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Order.Requests;

public class OrderCandleRequest
{
    [Required]
    public required int Id { get; set; }
    
    [Required]
    public required string Title { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int WeightGrams { get; set; }

    [Required]
    public required ImageRequest[] Images { get; set; }

    [Required]
    public required TypeCandleRequest TypeCandle { get; set; }
}
