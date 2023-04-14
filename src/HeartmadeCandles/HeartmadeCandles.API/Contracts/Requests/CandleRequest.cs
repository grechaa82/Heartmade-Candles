using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class CandleRequest
    {
        [JsonConstructor]
        public CandleRequest(
            string title, 
            string description, 
            string imageURL, 
            int weightGrams, 
            string typeCandle,
            bool isActive)
        {
            Title = title;
            Description = description;
            ImageURL = imageURL;
            WeightGrams = weightGrams;
            TypeCandle = typeCandle;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int WeightGrams { get; set; }
        public string TypeCandle { get; set; }
        public bool IsActive { get; set; }
    }
}
