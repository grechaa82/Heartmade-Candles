namespace HeartmadeCandles.Core.Models
{
    public class Customer : ModelBase
    {
        private Customer(
            string id,
            string name,
            string surname,
            string middleName,
            string phone,
            Address address,
            TypeDelivery typeDelivery)
        {
            Id = id;
            Name = name;
            Surname = surname;
            MiddleName = middleName;
            Phone = phone;
            Address = address;
            TypeDelivery = typeDelivery;
        }

        public string? Name { get; }
        public string? Surname { get; }
        public string? MiddleName { get; }
        public string? Phone { get; }
        public TypeDelivery TypeDelivery { get; }
        public Address? Address { get; }

        public static (Customer, ErrorDetail[]) Create(
            string name,
            string surname,
            string middleName,
            string phone,
            Address address,
            TypeDelivery typeDelivery,
            string id = null)
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                errorsMessage = $"'{nameof(name)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                errorsMessage = $"'{nameof(surname)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(middleName))
            {
                errorsMessage = $"'{nameof(middleName)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                errorsMessage = $"'{nameof(phone)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (address == null)
            {
                errorsMessage = $"'{nameof(address)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (typeDelivery == null)
            {
                errorsMessage = $"'{nameof(typeDelivery)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var customer = new Customer(
                id,
                name,
                surname,
                middleName,
                phone,
                address,
                typeDelivery);

            return (customer, errors.ToArray());
        }

        public static (Customer, ErrorDetail[]) ChangeTypeDelivery(Customer customer, TypeDelivery newTypeDelivery)
        {
            var (newCustomer, errors) = Customer.Create(
                customer.Name,
                customer.Surname,
                customer.MiddleName,
                customer.Phone,
                customer.Address,
                newTypeDelivery,
                customer.Id);

            return (newCustomer, errors);
        }
    }

    public enum TypeDelivery
    {
        Pickup,
        RussianPost,
        Sdek
    }
}
