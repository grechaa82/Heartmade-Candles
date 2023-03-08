namespace HeartmadeCandles.Core.Models
{
    public class Customer : ModelBase
    {
        public Customer(
    string name,
    string surname,
    string middleName,
    string phone,
    Address address,
    TypeDelivery typeDelivery = TypeDelivery.Pickup)
        {
            Name = name;
            Surname = surname;
            MiddleName = middleName;
            Phone = phone;
            TypeDelivery = typeDelivery;
            Address = address;
        }

        public string Name { get; }
        public string Surname { get; }
        public string MiddleName { get; }
        public string Phone { get; }
        public TypeDelivery TypeDelivery { get; }
        public Address Address { get; }
    }

    public enum TypeDelivery
    {
        Pickup,
        RussianPost,
        Sdek
    }
}
