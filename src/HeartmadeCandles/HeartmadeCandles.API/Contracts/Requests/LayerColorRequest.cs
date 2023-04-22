using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class LayerColorRequest
    {
        [JsonConstructor]
        public LayerColorRequest(
            string title, 
            string description, 
            decimal pricePerGram, 
            string imageURL, 
            bool isActive)
        {
            Title = title;
            Description = description;
            PricePerGram = pricePerGram;
            ImageURL = imageURL;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerGram { get; set; }
        public string ImageURL { get; set; }
        public bool IsActive { get; set; }
    }
}
