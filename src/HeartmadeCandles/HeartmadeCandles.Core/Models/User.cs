using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class User : ModelBase
    {
        private string _nickName;
        private string _email;
        private string _password;
        private string _customerId;
        private Role _role;

        [JsonConstructor]
        public User(
            string nickName,
            string email,
            string password,
            string customerId,
            string id = "",
            Role role = Role.Customer)
        {
            if (string.IsNullOrWhiteSpace(nickName))
            {
                throw new ArgumentNullException ($"'{nameof(nickName)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException($"'{nameof(email)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException($"'{nameof(password)}' connot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentNullException($"'{nameof(customerId)}' connot be null or whitespace.");
            }

            Id = id;
            _nickName = nickName;
            _email = email;
            _password = password;
            _customerId = customerId;
            _role = role;
        }

        public string NickName { get => _nickName; }
        public string Email { get => _email; }
        public string Password { get => _password; }
        public string CustomerId { get => _customerId; }
        public Role Role { get => _role; }
    }

    public enum Role
    {
        Customer,
        Admin
    }
}
