namespace HeartmadeCandles.Core.Models
{
    public class Customer : ModelBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string TypeDelivery { get; set; }
        public Address Address { get; set; }
    }
}
