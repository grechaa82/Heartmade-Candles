using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HeartmadeCandles.Core.Models
{
    public class Address : ModelBase
    {
        private string _country;
        private string _cities;
        private string _street;
        private string _house;
        private string _flat;
        private string _index;

        public Address(
            string id,
            string country,
            string cities,
            string street,
            string house,
            string flat,
            string index)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentNullException($"'{nameof(country)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(cities))
            {
                throw new ArgumentNullException($"'{nameof(cities)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentNullException($"'{nameof(street)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(house))
            {
                throw new ArgumentNullException($"'{nameof(house)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(flat))
            {
                throw new ArgumentNullException($"'{nameof(flat)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(index))
            {
                throw new ArgumentNullException($"'{nameof(index)}' connot be null or whitespace.");
            }

            Id = id;
            _country = country;
            _cities = cities;
            _street = street;
            _house = house;
            _flat = flat;
            _index  = index;
        }

        public string Country { get => _country; }
        public string Cities { get => _cities; }
        public string Street { get => _street; }
        public string House { get => _house; }
        public string Flat { get => _flat; }
        public string Index { get => _index; }
    }
}