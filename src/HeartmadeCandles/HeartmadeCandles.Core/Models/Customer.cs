using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class Customer : ModelBase
    {
        private string _name;
        private string _surname;
        private string _middleName;
        private string _phone;
        private TypeDelivery _typeDelivery;
        private Address _address;

        [JsonConstructor]
        public Customer(
            string name,
            string surname,
            string middleName,
            string phone,
            TypeDelivery typeDelivery,
            Address address,
            string id = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException($"'{nameof(name)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentNullException($"'{nameof(surname)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(middleName))
            {
                throw new ArgumentNullException($"'{nameof(middleName)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentNullException($"'{nameof(phone)}' connot be null or whitespace.");
            }

            if (address == null)
            {
                throw new ArgumentNullException($"'{nameof(address)}' connot be null or whitespace.");
            }

            if (typeDelivery == null)
            {
                throw new ArgumentNullException($"'{nameof(typeDelivery)}' connot be null or whitespace.");
            }

            Id = id;
            _name = name;
            _surname = surname;
            _middleName = middleName;
            _phone = phone;
            _typeDelivery = typeDelivery;
            _address  = address;
        }

        public string Name { get => _name; }
        public string Surname { get => _surname; }
        public string MiddleName { get => _middleName; }
        public string Phone { get => _phone; }
        public TypeDelivery TypeDelivery { get => _typeDelivery; }
        public Address Address { get => _address; }

        public static Customer ChangeTypeDelivery(Customer customer, TypeDelivery newTypeDelivery)
        {
            var newCustomer = new Customer(
                customer.Name,
                customer.Surname,
                customer.MiddleName,
                customer.Phone,
                newTypeDelivery,
                customer.Address,
                customer.Id);

            return newCustomer;
        }
    }

    public enum TypeDelivery
    {
        Pickup,
        RussianPost,
        Sdek
    }
}
