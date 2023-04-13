namespace HeartmadeCandles.Modules.Admin.Core.Models
{
    public class Candle
    {
        private int _id;
        private string _title;
        private string _description;
        private string _imageURL;
        private int _weightGrams;
        private bool _isUsed;
        private TypeCandle _typeCandle;
        private DateTime _createdAt;

        public Candle(
            int id, 
            string title, 
            string description, 
            string imageURL, 
            int weightGrams, 
            bool isUsed, 
            TypeCandle typeCandle)
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
            _isUsed = isUsed;
            _typeCandle = typeCandle;
            _createdAt = DateTime.UtcNow;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public string ImageURL { get => _imageURL; }
        public int WeightGrams { get => _weightGrams; }
        public bool IsUsed { get => _isUsed; }
        public TypeCandle TypeCandle { get => _typeCandle; }
        public DateTime CreatedAt { get => _createdAt; }
    }
}
