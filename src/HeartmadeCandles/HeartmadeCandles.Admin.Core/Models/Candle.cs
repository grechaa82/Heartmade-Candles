using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Candle
    {
        private const int MaxTitleLenght = 48;
        private const int MaxDescriptionLenght = 256;
        
        private int _id;
        private string _title;
        private string _description;
        private int _weightGrams;
        private string _imageURL;
        private bool _isActive;
        private TypeCandle _typeCandle;
        private DateTime _createdAt;

        private Candle(
            int id,
            string title,
            string description,
            int weightGrams,
            string imageURL,
            bool isActive,
            TypeCandle typeCandle,
            DateTime createdAt)
        {
            _id = id;
            _title = title;
            _description = description;
            _weightGrams = weightGrams;
            _imageURL = imageURL;
            _isActive = isActive;
            _typeCandle = typeCandle;
            _createdAt = createdAt;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public int WeightGrams { get => _weightGrams; }
        public string ImageURL { get => _imageURL; }
        public bool IsActive { get => _isActive; }
        public TypeCandle TypeCandle { get => _typeCandle; }
        public DateTime CreatedAt { get => _createdAt; }
        
        public static Result<Candle> Create(
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
                return Result.Failure<Candle>($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (title.Length > MaxTitleLenght)
            {
                return Result.Failure<Candle>($"'{nameof(title)}' connot be more than {MaxTitleLenght} characters.");
            }

            if (description == null)
            {
                return Result.Failure<Candle>($"'{nameof(description)}' connot be null.");
            }

            if (description.Length > MaxDescriptionLenght)
            {
                return Result.Failure<Candle>($"'{nameof(description)}' connot be more than {MaxDescriptionLenght} characters.");
            }

            if (weightGrams <= 0)
            {
                return Result.Failure<Candle>($"'{nameof(weightGrams)}' сannot be 0 or less.");
            }
            
            var candle = new Candle(
                id,
                title,
                description,
                weightGrams,
                imageURL,
                isActive,
                typeCandle,
                createdAt ?? DateTime.UtcNow);

            return Result.Success(candle);
        }
    }
}
