using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class CandleMinimal : ModelBase
    {
        private string _title ;
        private string _imageURL ;
        private TypeCandle _typeCandle;
        private string _description;

        [JsonConstructor]
        public CandleMinimal(
            string title, 
            string imageURL,
            TypeCandle typeCandle,
            string id = "",
            string description ="")
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (typeCandle == null)
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null.");
            }

            Id = id;
            _title  = title;
            _imageURL  = imageURL;
            _typeCandle = typeCandle;
            _description  = description;
        }

        public string Title { get => _title ; }
        public string ImageURL { get => _imageURL ; }
        public TypeCandle TypeCandle { get => _typeCandle; }
        public string Description { get => _description; }
    }
}
