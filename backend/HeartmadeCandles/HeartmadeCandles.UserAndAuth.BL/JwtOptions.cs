namespace HeartmadeCandles.UserAndAuth.BL;

public class JwtOptions
{
    public required string SecretKey { get; set; }

    public double ExpiryInMinutes { get; set; }

    public required string Issuer { get; set; }

    public required string Audience { get; set; }
}