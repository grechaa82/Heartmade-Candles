using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class LayerColorRequest
{
    [Required] public required string Title { get; set; }

    [Required] public required string Description { get; set; }

    [Required] public decimal PricePerGram { get; set; }

    public required ImageRequest[] Images { get; set; }

    [Required] public bool IsActive { get; set; }
}