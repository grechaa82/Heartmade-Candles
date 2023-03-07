namespace HeartmadeCandles.Core.Models
{
    public class User : ModelBase
    {
        public User(string nickName,
            string email,
            string password,
            Customer customer = null,
            Role role = Role.Customer)
        {
            NickName = nickName;
            Email = email;
            Password = password;
            Role = role;
        }

        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Customer? Customer { get; set; }
        public Role Role { get; set; } = Role.Customer;
    }

    public enum Role
    {
        Customer,
        Admin
    }
}