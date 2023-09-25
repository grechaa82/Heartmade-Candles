using HeartmadeCandles.Auth.Core;

namespace HeartmadeCandles.Auth.BL;

public class AuthService : IAuthService
{
    private static string _validLogin;
    private static string _validHashPassword;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _validLogin = Environment.GetEnvironmentVariable("VAR_ADMIN_LOGIN");
        _validHashPassword = _passwordHasher.Generate(Environment.GetEnvironmentVariable("VAR_ADMIN_PASSWORD"));
    }

    public bool IsValidUser(string login, string password)
    {
        return login == _validLogin && _passwordHasher.Verify(password, _validHashPassword);
    }
}