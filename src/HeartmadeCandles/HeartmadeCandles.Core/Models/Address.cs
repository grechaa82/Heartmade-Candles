namespace HeartmadeCandles.Core.Models
{
    public class Address : ModelBase
    {
        public Address(
    string country,
    string cities,
    string street,
    string house,
    string flat,
    string index)
        {
            Country = country;
            Cities = cities;
            Street = street;
            House = house;
            Flat = flat;
            Index = index;
        }

        public string Country { get; }
        public string Cities { get; }
        public string Street { get; }
        public string House { get; }
        public string Flat { get; }
        public string Index { get; }
    }
}