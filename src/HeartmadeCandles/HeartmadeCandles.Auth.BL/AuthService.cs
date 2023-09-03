using HeartmadeCandles.Auth.Core;

namespace HeartmadeCandles.Auth.BL
{
    public class AuthService : IAuthService
    {
        private readonly string validLogin = "admin";
        private readonly string validPassword = "admin";

        public bool IsValidUser(string login, string password)
        {
            return (login == validLogin && password == validPassword);
        }
    }
}
