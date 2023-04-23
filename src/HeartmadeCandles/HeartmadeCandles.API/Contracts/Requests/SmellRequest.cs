using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class SmellRequest
    {
        [JsonConstructor]
        public SmellRequest(
            string title, 
            string description, 
            decimal price, 
            bool isActive)
        {
            Title = title;
            Description = description;
            Price = price;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
