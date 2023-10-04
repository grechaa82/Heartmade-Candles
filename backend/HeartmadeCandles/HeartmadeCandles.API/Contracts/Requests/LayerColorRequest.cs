using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class LayerColorRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal PricePerGram { get; set; }
    public ImageRequest[] Images { get; set; }
    [Required]
    public bool IsActive { get; set; }
}