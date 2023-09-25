namespace HeartmadeCandles.Auth.Core;

public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hashPassword);
}