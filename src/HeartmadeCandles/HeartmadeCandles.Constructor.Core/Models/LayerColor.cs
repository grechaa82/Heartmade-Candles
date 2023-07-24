namespace HeartmadeCandles.Constructor.Core.Models
{
    public class LayerColor
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerGram { get; set; }
        public Image[] Images { get; set; }
    }
}