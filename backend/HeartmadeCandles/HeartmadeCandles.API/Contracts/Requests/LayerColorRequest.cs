using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests;

public class LayerColorRequest
{
    [JsonConstructor]
    public LayerColorRequest(
        string title,
        string description,
        decimal pricePerGram,
        ImageRequest[] images,
        bool isActive)
    {
        Title = title;
        Description = description;
        PricePerGram = pricePerGram;
        Images = images;
        IsActive = isActive;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public decimal PricePerGram { get; set; }
    public ImageRequest[] Images { get; set; }
    public bool IsActive { get; set; }
}