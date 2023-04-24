using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class CandleRequest
    {
        [JsonConstructor]
        public CandleRequest(
            string title, 
            string description, 
            int weightGrams, 
            string imageURL, 
            string typeCandle,
            bool isActive)
        {
            Title = title;
            Description = description;
            WeightGrams = weightGrams;
            ImageURL = imageURL;
            TypeCandle = typeCandle;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public int WeightGrams { get; set; }
        public string ImageURL { get; set; }
        public string TypeCandle { get; set; }
        public bool IsActive { get; set; }
    }
}
