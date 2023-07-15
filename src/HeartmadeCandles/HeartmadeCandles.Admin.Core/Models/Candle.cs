using CSharpFunctionalExtensions;
using Microsoft.VisualBasic;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Candle
    {
        public const int MaxTitleLength = 48;
        public const int MaxDescriptionLength = 256;

        private int _id;
        private string _title;
        private string _description;
        private decimal _price;
        private int _weightGrams;
        private string _imageURL;
        private bool _isActive;
        private TypeCandle _typeCandle;
        private DateTime _createdAt;

        private Candle(
            int id,
            string title,
            string description,
            decimal price,
            int weightGrams,
            string imageURL,
            bool isActive,
            TypeCandle typeCandle,
            DateTime createdAt)
        {
            _id = id;
            _title = title;
            _description = description;
            _price = price;
            _weightGrams = weightGrams;
            _imageURL = imageURL;
            _isActive = isActive;
            _typeCandle = typeCandle;
            _createdAt = createdAt;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public decimal Price { get => _price; }
        public int WeightGrams { get => _weightGrams; }
        public string ImageURL { get => _imageURL; }
        public bool IsActive { get => _isActive; }
        public TypeCandle TypeCandle { get => _typeCandle; }
        public DateTime CreatedAt { get => _createdAt; }

        public static Result<Candle> Create(
            string title,
            string description,
            decimal price,
            int weightGrams,
            string imageURL,
            TypeCandle typeCandle,
            bool isActive = true,
            int id = 0,
            DateTime? createdAt = null)
        {
            var result = Result.Success();

            if (string.IsNullOrWhiteSpace(title))
            {
                result = Result.Combine(
                   result, 
                   Result.Failure<Candle>($"'{nameof(title)}' cannot be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLength)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Candle>($"'{nameof(title)}' cannot be more than {MaxTitleLength} characters"));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Candle>($"'{nameof(description)}' cannot be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(description) && description.Length > MaxDescriptionLength)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Candle>($"'{nameof(description)}' cannot be more than {MaxDescriptionLength} characters"));
            }

            if (price <= 0)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Candle>($"'{nameof(price)}' cannot be 0 or less"));
            }

            if (weightGrams <= 0)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Candle>($"'{nameof(weightGrams)}' cannot be 0 or less"));
            }

            if (typeCandle == null)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Candle>($"'{nameof(typeCandle)}' cannot be null"));
            }

            if (result.IsFailure)
            {
                return Result.Failure<Candle>(result.Error);
            }

            var candle = new Candle(
                id,
                title,
                description,
                price,
                weightGrams,
                imageURL,
                isActive,
                typeCandle,
                createdAt ?? DateTime.UtcNow);

            return Result.Success(candle);
        }

        public Result<Candle> RemoveURLsFromImageURL (string[] imageURLToRemove)
        {
            var urlsList = _imageURL.Split(',').ToList();

            foreach (string url in imageURLToRemove)
            {
                if (urlsList.Contains(url))
                {
                    urlsList.Remove(url);
                }
            }

            var newImagesURL = string.Join(",", urlsList);

            var candle = new Candle(
                _id,
                _title,
                _description,
                _price,
                _weightGrams,
                newImagesURL,
                _isActive,
                _typeCandle,
                _createdAt);

            return Result.Success(candle);
        }
    }
}
