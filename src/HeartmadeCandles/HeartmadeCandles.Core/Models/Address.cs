namespace HeartmadeCandles.Core.Models
{
    public class Address : ModelBase
    {
        private Address(
            string id,
            string country,
            string cities,
            string street,
            string house,
            string flat,
            string index)
        {
            Id = id;
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

        public static (Address, ErrorDetail[]) Create(
            string country,
            string cities,
            string street,
            string house,
            string flat,
            string index,
            string id = null)
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(country))
            {
                errorsMessage = $"'{nameof(country)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(cities))
            {
                errorsMessage = $"'{nameof(cities)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(street))
            {
                errorsMessage = $"'{nameof(street)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(house))
            {
                errorsMessage = $"'{nameof(house)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(flat))
            {
                errorsMessage = $"'{nameof(flat)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(index))
            {
                errorsMessage = $"'{nameof(index)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var address = new Address(
                id,
                country,
                cities,
                street,
                house,
                flat,
                index);

            return (address, errors.ToArray());
        }
    }
}