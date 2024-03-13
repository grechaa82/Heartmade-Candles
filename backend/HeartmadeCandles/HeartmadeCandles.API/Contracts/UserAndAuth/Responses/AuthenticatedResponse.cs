using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Auth.Responses;

public class AuthenticatedResponse
{
    public int UserId { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string AccessToken { get; set; }

    [Required]
    public required string RefreshToken { get; set; }

    public DateTime ExpireAt { get; set; }
}
