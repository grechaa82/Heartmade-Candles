using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class DecorRequest
{
    [Required] public required string Title { get; set; }

    [Required] public required string Description { get; set; }

    [Required] public decimal Price { get; set; }

    public required ImageRequest[] Images { get; set; }

    [Required] public bool IsActive { get; set; }
}