using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class Candle : ModelBase
    {
        private bool _isUsed;
        private string _title;
        private string _imageURL;
        private List<int> _numberOfLayers;
        private List<LayerColor> _layerColors;
        private List<Smell> _smells;
        private List<Decor> _decors;
        private int _weightGrams;
        private TypeCandle _typeCandle;
        private string _description;

        [JsonConstructor]
        public Candle(
            bool isUsed,
            string title,
            string imageURL,
            List<int> numberOfLayers,
            List<LayerColor> layerColors,
            List<Smell> smells,
            List<Decor> decors,
            int weightGrams,
            TypeCandle typeCandle,
            string id = "",
            string description = "")
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (weightGrams <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(weightGrams)}' сannot be 0 or less.");
            }

            Id = id;
            _isUsed = isUsed;
            _title = title;
            _imageURL = imageURL;
            _numberOfLayers = numberOfLayers;
            _layerColors = layerColors;
            _smells = smells;
            _decors = decors;
            _weightGrams = weightGrams;
            _typeCandle = typeCandle;
            _description = description;
        }

        public bool IsUsed { get => _isUsed; }
        public string Title { get => _title; }
        public string ImageURL { get => _imageURL; }
        public List<int> NumberOfLayers { get => _numberOfLayers; }
        public List<LayerColor> LayerColors { get => _layerColors; }
        public List<Smell> Smells { get => _smells; }
        public List<Decor> Decors { get => _decors; }
        public int WeightGrams { get => _weightGrams; }
        public TypeCandle TypeCandle { get => _typeCandle; }
        public string Description { get => _description; }

        public static decimal GetPrice(Candle candle)
        {
            var gramsInLayer = candle.WeightGrams / candle.NumberOfLayers.First();

            var price = 0m;

            foreach (var layer in candle.LayerColors)
            {
                price = layer.PricePerGram * gramsInLayer;
            }

            return price;
        }
    }

    public enum TypeCandle
    {
        ContainerCandle,
        ShapedCandle
    }
}
