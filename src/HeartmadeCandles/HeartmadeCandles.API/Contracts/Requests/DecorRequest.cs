using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class DecorRequest
    {
        [JsonConstructor]
        public DecorRequest(
            string title, 
            string description, 
            decimal price,
            ImageRequest[] images, 
            bool isActive)
        {
            Title = title;
            Description = description;
            Price = price;
            Images = images;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ImageRequest[] Images { get; set; }
        public bool IsActive { get; set; }
    }
}
