using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Decor
    {
        public const int MaxTitleLenght = 48;
        public const int MaxDescriptionLenght = 256;

        private int _id;
        private string _title;
        private string _description;
        private decimal _price;
        private string _imageURL;
        private bool _isActive;

        private Decor(
            int id,
            string title,
            string description,
            decimal price,
            string imageURL,
            bool isActive)
        {
            _id = id;
            _title = title;
            _description = description;
            _price = price;
            _imageURL = imageURL;
            _isActive = isActive;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public decimal Price { get => _price; }
        public string ImageURL { get => _imageURL; }
        public bool IsActive { get => _isActive; }

        public static Result<Decor> Create(
            string title,
            string description,
            decimal price,
            string imageURL,
            bool isActive = true,
            int id = 0)
        {
            var result = Result.Success();

            if (string.IsNullOrWhiteSpace(title))
            {
                result = Result.Combine(
                    result,
                    Result.Failure<Decor>($"'{nameof(title)}' cannot be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLenght)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<Decor>($"'{nameof(title)}' cannot be more than {MaxTitleLenght} characters"));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                result = Result.Combine(
                    result,
                    Result.Failure<Decor>($"'{nameof(description)}' cannot be null"));
            }

            if (!string.IsNullOrWhiteSpace(description) && description.Length > MaxDescriptionLenght)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<Decor>($"'{nameof(description)}' cannot be more than {MaxDescriptionLenght} characters"));
            }

            if (price <= 0)
            {
                result = Result.Combine(
                    result,
                    Result.Failure<Decor>($"'{nameof(price)}' сannot be 0 or less"));
            }

            if (result.IsFailure)
            {
                return Result.Failure<Decor>(result.Error);
            }

            var decor = new Decor(
                id,
                title,
                description,
                price,
                imageURL,
                isActive);

            return Result.Success(decor);
        }
    }
}
