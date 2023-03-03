namespace HeartmadeCandles.Core.Models
{
    public class Candle : ModelBase
    {
        public bool IsUsed { get; set; }

        public string? Title { get; set; }

        public string? ImageURL { get; set; }

        public List<int>? NumberOfLayers { get; set; }

        public List<LayerColor>? LayerColors { get; set; }

        public List<Smell>? Smells { get; set; }

        public List<Decor>? Decors { get; set; }
    }
}
