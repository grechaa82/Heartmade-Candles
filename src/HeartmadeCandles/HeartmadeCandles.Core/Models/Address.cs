namespace HeartmadeCandles.Core.Models
{
    public class Address : ModelBase
    {
        public string Country { get; set; }
        public string Cities { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Flat { get; set; }
        public string Index { get; set; }
    }
}