using System.Text.Json.Serialization;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Candle
    {
        private int _id;
        private string _title;
        private string _description;
        private int _weightGrams;
        private string _imageURL;
        private bool _isActive;
        private TypeCandle _typeCandle;
        private DateTime _createdAt;

        [JsonConstructor]
        public Candle(
            string title,
            string description,
            int weightGrams,
            string imageURL,
            bool isActive = true,
            TypeCandle typeCandle = TypeCandle.OtherCandle,
            int id = 0,
            DateTime? createdAt = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (title.Length > 48)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(title)}' connot be more than 64 characters.");
            }

            if (description.Length > 256)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(description)}' connot be more than 256 characters.");
            }

            if (weightGrams <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(weightGrams)}' сannot be 0 or less.");
            }

            _id = id;
            _title = title;
            _description = description;
            _weightGrams = weightGrams;
            _imageURL = imageURL;
            _isActive = isActive;
            _typeCandle = typeCandle;
            _createdAt = createdAt ?? DateTime.UtcNow;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public int WeightGrams { get => _weightGrams; }
        public string ImageURL { get => _imageURL; }
        public bool IsActive { get => _isActive; }
        public TypeCandle TypeCandle { get => _typeCandle; }
        public DateTime CreatedAt { get => _createdAt; }
    }
}
