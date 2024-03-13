using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.BL.Services;

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

    public bool IsValidUser(string email, string password)
    {
        return email == _validLogin && _passwordHasher.Verify(password, _validHashPassword);
    }

    public Task<Result<Token>> CreateToken(User user)
    {
        throw new NotImplementedException();
    }
}