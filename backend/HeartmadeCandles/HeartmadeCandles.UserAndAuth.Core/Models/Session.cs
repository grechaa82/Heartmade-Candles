namespace HeartmadeCandles.UserAndAuth.Core.Models;

public class Session
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public int UserId { get; init; }

    public User? User { get; init; }

    public required string RefreshToken { get; init; }

    public DateTime ExpireAt { get; init; }
}
