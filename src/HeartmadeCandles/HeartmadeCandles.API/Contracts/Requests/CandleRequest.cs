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
            string imageURL, 
            TypeCandleRequest typeCandle,
            bool isActive)
        {
            Title = title;
            Description = description;
            Price = price;
            WeightGrams = weightGrams;
            ImageURL = imageURL;
            TypeCandle = typeCandle;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int WeightGrams { get; set; }
        public string ImageURL { get; set; }
        public TypeCandleRequest TypeCandle { get; set; }
        public bool IsActive { get; set; }
    }
}
