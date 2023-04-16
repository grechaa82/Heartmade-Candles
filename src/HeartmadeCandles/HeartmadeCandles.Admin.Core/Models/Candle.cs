using System.Text.Json.Serialization;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Candle
    {
        private int _id;
        private string _title;
        private string _description;
        private string _imageURL;
        private int _weightGrams;
        private bool _isActive;
        private TypeCandle _typeCandle;
        private DateTime _createdAt;

        [JsonConstructor]
        public Candle(
            string title,
            string description,
            string imageURL,
            int weightGrams,
            bool isActive = true,
            TypeCandle typeCandle = TypeCandle.OtherCandle,
            int id = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (weightGrams <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(weightGrams)}' сannot be 0 or less.");
            }

            if (typeCandle == null)
            {
                throw new ArgumentNullException($"'{nameof(typeCandle)}' connot be null.");
            }

            _id = id;
            _title = title;
            _description = description;
            _imageURL = imageURL;
            _weightGrams = weightGrams;
            _isActive = isActive;
            _typeCandle = typeCandle;
            _createdAt = DateTime.UtcNow;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public string ImageURL { get => _imageURL; }
        public int WeightGrams { get => _weightGrams; }
        public bool IsActive { get => _isActive; }
        public TypeCandle TypeCandle { get => _typeCandle; }
        public DateTime CreatedAt { get => _createdAt; }
    }
}
