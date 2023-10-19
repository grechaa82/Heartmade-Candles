using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class CandleRequest
{
    [Required] public required string Title { get; set; }

    [Required] public required string Description { get; set; }

    [Required] public decimal Price { get; set; }

    [Required] public int WeightGrams { get; set; }

    public required ImageRequest[] Images { get; set; }

    [Required] public required TypeCandleRequest TypeCandle { get; set; }

    [Required] public bool IsActive { get; set; }
}