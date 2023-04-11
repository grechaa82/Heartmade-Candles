using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class Smell : ModelBase
    {
        private bool _isUsed;
        private string _title;
        private decimal _price;
        private string _description;

        [JsonConstructor]
        public Smell(
            bool isUsed,
            string title,
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
            _price = price;
            _description = description;
        }

        public bool IsUsed { get => _isUsed; }
        public string Title { get => _title; }
        public decimal Price { get => _price; }
        public string Description { get => _description; }
    }
}
