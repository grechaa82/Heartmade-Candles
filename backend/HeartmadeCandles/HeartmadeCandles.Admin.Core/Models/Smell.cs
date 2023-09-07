using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Smell
    {
        public const int MaxTitleLenght = 48;
        public const int MaxDescriptionLenght = 256;
        
        private int _id;
        private string _title;
        private string _description;
        private decimal _price;
        private bool _isActive;

        private Smell(
            int id,
            string title, 
            string description, 
            decimal price, 
            bool isActive)
        {
            _id = id;
            _title = title;
            _description = description;
            _price = price;
            _isActive = isActive;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public decimal Price { get => _price; }
        public bool IsActive { get => _isActive; }
        
        public static Result<Smell> Create(
            string title, 
            string description, 
            decimal price, 
            bool isActive,
            int id = 0)
        {
            var result = Result.Success();

            if (string.IsNullOrWhiteSpace(title))
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Smell>($"'{nameof(title)}' cannot be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLenght)
            {
                result = Result.Combine(
                   result,
                   Result.Failure<Smell>($"'{nameof(title)}' cannot be more than {MaxTitleLenght} characters"));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                result = Result.Combine(
                    result,
                    Result.Failure<Smell>($"'{nameof(description)}' cannot be null or whitespace"));
            }

            if (!string.IsNullOrWhiteSpace(description) && description.Length > MaxDescriptionLenght)
            {
                result = Result.Combine(
                  result,
                  Result.Failure<Smell>($"'{nameof(description)}' cannot be more than {MaxDescriptionLenght} characters"));
            }

            if (price <= 0)
            {
                result = Result.Combine(
                  result,
                  Result.Failure<Smell>($"'{nameof(price)}' сannot be 0 or less"));
            }
            
            var smell = new Smell(
                id, 
                title, 
                description, 
                price, 
                isActive);

            return Result.Success(smell);
        }
    }
}
