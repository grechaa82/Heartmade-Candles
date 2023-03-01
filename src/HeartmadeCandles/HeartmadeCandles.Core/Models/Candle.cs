namespace HeartmadeCandles.Core.Models
{
    public class Candle
    {
        public string? Id { get; set; }

        public bool IsUsed { get; set; }

        public string? Name { get; set; }

        public string? ImageURL { get; set; }

        public List<int>? NumberOfLayers { get; set; }

        public List<LayerColor>? LayerColors { get; set; }

        public List<Smell>? Smells { get; set; }

        public List<Decor>? Decors { get; set; }
    }
}
