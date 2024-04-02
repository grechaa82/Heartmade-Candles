namespace HeartmadeCandles.UserAndAuth.BL;

public class JwtOptions
{
    public required string SecretKey { get; set; }

    public double ExpirationOfAccessTokenInMinutes { get; set; }

    public double ExpirationOfRefreshTokenInMinutes { get; set; }

    public required string Issuer { get; set; }

    public required string Audience { get; set; }
}