namespace HeartmadeCandles.UserAndAuth.Core.Models;

public class User
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public string Nickname { get; set; }

    public required string PasswordHash { get; set; }

    public DateTime RegistrationData { get; set; }
}
