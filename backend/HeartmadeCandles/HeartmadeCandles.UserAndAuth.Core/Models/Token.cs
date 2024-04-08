namespace HeartmadeCandles.UserAndAuth.Core.Models;

public class Token
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }

    public DateTime ExpireAt { get; init; }
}
