namespace HeartmadeCandles.Auth.Core.Models;

public class Token
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }

    public DateTime ExpireTime { get; set; }
}
