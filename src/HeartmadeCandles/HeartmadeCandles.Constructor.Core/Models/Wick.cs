namespace HeartmadeCandles.Constructor.Core.Models
{
    public class Wick
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Image[] Images { get; set; }
    }
}