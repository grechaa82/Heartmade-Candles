using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class CandleRequest
    {
        [JsonConstructor]
        public CandleRequest(
            string title, 
            string description,
            decimal price,
            int weightGrams, 
            ImageRequest[] image, 
            TypeCandleRequest typeCandle,
            bool isActive)
        {
            Title = title;
            Description = description;
            Price = price;
            WeightGrams = weightGrams;
            Images = image;
            TypeCandle = typeCandle;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int WeightGrams { get; set; }
        public ImageRequest[] Images { get; set; }
        public TypeCandleRequest TypeCandle { get; set; }
        public bool IsActive { get; set; }
    }
}
