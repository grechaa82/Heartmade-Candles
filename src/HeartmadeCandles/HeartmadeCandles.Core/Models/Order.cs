namespace HeartmadeCandles.Core.Models
{
    public class Order : ModelBase
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string TypeDelivery { get; set; }
        public DateTime CreatedAt { get; set; }
        public Customer Customer { get; set; }
        public string Title { get; set; }
        public int NumberOfLayers { get; set; }
        public Dictionary<int, LayerColor> LayerColors { get; set; }
        public Smell Smell { get; set; }
        public Decor Decor { get; set; }
        public string Description { get; set; }
    }
}
