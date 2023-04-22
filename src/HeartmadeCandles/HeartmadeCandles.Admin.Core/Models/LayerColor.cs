namespace HeartmadeCandles.Admin.Core.Models
{
    public class LayerColor
    {
        private int _id;
        private string _title;
        private string _description;
        private decimal _pricePerGram;
        private string _imageURL;
        private bool _isActive;

        public LayerColor(
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

            if (title.Length > 48)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(title)}' connot be more than 64 characters.");
            }

            if (description.Length > 256)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(title)}' connot be more than 256 characters.");
            }

            if (pricePerGram <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(pricePerGram)}' сannot be 0 or less.");
            }

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
    }
}
