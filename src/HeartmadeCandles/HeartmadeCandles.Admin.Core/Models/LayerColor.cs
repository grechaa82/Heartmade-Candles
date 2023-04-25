using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class LayerColor
    {
        private const int MaxTitleLenght = 48;
        private const int MaxDescriptionLenght = 256;
        
        private int _id;
        private string _title;
        private string _description;
        private decimal _pricePerGram;
        private string _imageURL;
        private bool _isActive;

        private LayerColor(
            int id,
            string title, 
            string description, 
            decimal pricePerGram, 
            string imageURL, 
            bool isActive)
        {
            _id = id;
            _title = title;
            _description = description;
            _pricePerGram = pricePerGram;
            _imageURL = imageURL;
            _isActive = isActive;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public decimal PricePerGram { get => _pricePerGram; }
        public string ImageURL { get => _imageURL; }
        public bool IsActive { get => _isActive; }
        
        public static Result<LayerColor> Create(
            string title,
            string description,
            decimal pricePerGram,
            string imageURL,
            bool isActive = true,
            int id = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure<LayerColor>($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (title.Length > MaxTitleLenght)
            {
                return Result.Failure<LayerColor>($"'{nameof(title)}' connot be more than {MaxTitleLenght} characters.");
            }

            if (description == null)
            {
                return Result.Failure<LayerColor>($"'{nameof(description)}' connot be null.");
            }

            if (description.Length > MaxDescriptionLenght)
            {
                return Result.Failure<LayerColor>($"'{nameof(description)}' connot be more than {MaxDescriptionLenght} characters.");
            }

            if (pricePerGram <= 0)
            {
                return Result.Failure<LayerColor>($"'{nameof(pricePerGram)}' сannot be 0 or less.");
            }
            
            var layerColor = new LayerColor(
                id, 
                title, 
                description, 
                pricePerGram, 
                imageURL, 
                isActive);

            return Result.Success(layerColor);
        }
    }
}
