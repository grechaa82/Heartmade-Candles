namespace HeartmadeCandles.API;

public class JwtOptions
{
    public string SecretKey { get; set; }
    public double ExpiryInMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}