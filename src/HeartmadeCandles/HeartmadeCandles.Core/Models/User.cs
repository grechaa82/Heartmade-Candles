namespace HeartmadeCandles.Core.Models
{
    public class User : ModelBase
    {
        public User(
            string nickName, 
            string email,
            string password,
            string customerId,
            Role role = Role.Customer)
        {
            NickName = nickName;
            Email = email;
            Password = password;
            CustomerId = customerId;
            Role = role;
        }

        public string NickName { get; }
        public string Email { get; }
        public string Password { get; }
        public string CustomerId { get; }
        public Role Role { get; } = Role.Customer;
    }

    public enum Role
    {
        Customer,
        Admin
    }
}