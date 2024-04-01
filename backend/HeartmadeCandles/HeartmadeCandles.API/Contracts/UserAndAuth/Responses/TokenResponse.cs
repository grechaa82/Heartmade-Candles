using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Auth.Responses;

public class TokenResponse
{
    [Required]
    public required string AccessToken { get; set; }

    [Required]
    public required string RefreshToken { get; set; }

    [Required]
    public DateTime ExpireAt { get; set; }
}
