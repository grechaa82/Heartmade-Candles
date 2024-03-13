using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.UserAndAuth.Requests;

public class CreateUserRequest
{
    public string UserName { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
