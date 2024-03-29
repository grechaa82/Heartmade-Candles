using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.UserAndAuth.Requests;

public class TokenRequest
{
    [Required]
    public required string AccessToken { get; set; }

    [Required]
    public required string RefreshToken { get; set; }
}
