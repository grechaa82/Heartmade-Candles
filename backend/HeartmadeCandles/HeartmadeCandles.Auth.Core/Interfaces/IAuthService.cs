namespace HeartmadeCandles.Auth.Core.Interfaces;

public interface IAuthService
{
    bool IsValidUser(string login, string password);
}