using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class CandleRequest
{
    [Required] public string Title { get; set; }

    [Required] public string Description { get; set; }

    [Required] public decimal Price { get; set; }

    [Required] public int WeightGrams { get; set; }

    public ImageRequest[] Images { get; set; }

    [Required] public TypeCandleRequest TypeCandle { get; set; }

    [Required] public bool IsActive { get; set; }
}