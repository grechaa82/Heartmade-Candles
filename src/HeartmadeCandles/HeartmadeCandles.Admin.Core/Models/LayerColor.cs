using System;

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
        
        public static LayerColor Create(
            string title,
            string description,
            decimal pricePerGram,
            string imageURL,
            bool isActive = true,
            int id = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (title.Length > MaxTitleLenght)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(title)}' connot be more than 64 characters.");
            }

            if (description.Length > MaxDescriptionLenght)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(description)}' connot be more than 256 characters.");
            }

            if (pricePerGram <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(pricePerGram)}' сannot be 0 or less.");
            }
            
            return new LayerColor(
                id, 
                title, 
                description, 
                pricePerGram, 
                imageURL, 
                isActive);
        }
    }
}
