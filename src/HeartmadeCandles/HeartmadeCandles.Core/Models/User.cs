namespace HeartmadeCandles.Core.Models
{
    public class User : ModelBase
    {
        private User(
            string id,
            string nickName,
            string email,
            string password,
            string customerId,
            Role role = Role.Customer)
        {
            Id = id;
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
        public Role Role { get; }

        public static (User, ErrorDetail[]) Create(
            string nickName,
            string email,
            string password,
            string customerId,
            Role role = Role.Customer,
            string id = null)
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(nickName))
            {
                errorsMessage = $"'{nameof(nickName)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                errorsMessage = $"'{nameof(email)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorsMessage = $"'{nameof(password)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (string.IsNullOrWhiteSpace(customerId))
            {
                errorsMessage = $"'{nameof(customerId)}' connot be null or whitespace.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var user = new User(
                id,
                nickName,
                email,
                password,
                customerId,
                role);

            return (user, errors.ToArray());
        }
    }

    public enum Role
    {
        Customer,
        Admin
    }
}
