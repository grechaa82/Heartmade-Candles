namespace HeartmadeCandles.Admin.Core.Models
{
    public class Decor
    {
        private int _id;
        private string _title;
        private string _description;
        private decimal _price;
        private string _imageURL;
        private bool _isActive;

        public Decor(
            string title, 
            string description, 
            decimal price, 
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

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(price)}' сannot be 0 or less.");
            }

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
    }
}
