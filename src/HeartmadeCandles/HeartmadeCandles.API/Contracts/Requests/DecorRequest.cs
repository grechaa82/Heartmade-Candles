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
            string imageURL, 
            bool isActive)
        {
            Title = title;
            Description = description;
            Price = price;
            ImageURL = imageURL;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public bool IsActive { get; set; }
    }
}
