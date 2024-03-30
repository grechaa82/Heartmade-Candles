namespace HeartmadeCandles.UserAndAuth.Core.Models;

[Serializable]
public class TokenPayload
{
    public int UserId { get; init; }

    public required string UserName { get; init; }

    public Role Role { get; init; }
}
