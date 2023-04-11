using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class LayerColor : ModelBase
    {
        private bool _isUsed;
        private string _title;
        private string _hex;
        private string _imageURL;
        private decimal _pricePerGram;
        private string _description;

        [JsonConstructor]
        public LayerColor(
            string id,
            bool isUsed,
            string title,
            string hEX,
            string imageURL,
            decimal pricePerGram,
            string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (pricePerGram <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(pricePerGram)}' сannot be 0 or less.");
            }

            Id = id;
            _isUsed = isUsed;
            _title = title;
            _hex = hEX;
            _imageURL = imageURL;
            _pricePerGram = pricePerGram;
            _description  = description;
        }

        public bool IsUsed { get => _isUsed; }
        public string Title { get => _title; }
        public string HEX { get => _hex; }
        public string ImageURL { get => _imageURL; }
        public decimal PricePerGram { get => _pricePerGram; }
        public string Description { get => _description; }
    }
}
