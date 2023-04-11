using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class Decor : ModelBase
    {
        private bool _isUsed;
        private string _title;
        private string _imageURL;
        private decimal _price;
        private string _description;

        [JsonConstructor]
        public Decor(
            bool isUsed,
            string title,
            string imageURL,
            decimal price,
            string id = "",
            string description = "")
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(price)}' сannot be 0 or less.");
            }

            Id = id;
            _isUsed = isUsed;
            _title = title;
            _imageURL = imageURL;
            _price = price;
            _description  = description;
        }

        public bool IsUsed { get => _isUsed; }
        public string Title { get => _title; }
        public string ImageURL { get => _imageURL; }
        public decimal Price { get => _price; }
        public string Description { get => _description; }
    }
}
