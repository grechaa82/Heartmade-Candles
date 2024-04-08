using HeartmadeCandles.UserAndAuth.Core.Interfaces;

namespace HeartmadeCandles.UserAndAuth.BL.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 15);
    }

    public bool Verify(string password, string hashPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }
}