namespace HeartmadeCandles.Auth.Core;

public interface IAuthService
{
    bool IsValidUser(string login, string password);
}