using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.Core.Interfaces;

public interface IAuthService
{
    bool IsValidUser(string email, string password);

    Task<Result<Token>> CreateToken(User user);
}