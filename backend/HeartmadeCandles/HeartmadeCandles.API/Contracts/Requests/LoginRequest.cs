using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class LoginRequest
{
    [Required] public required string Login { get; set; }

    [Required] public required string Password { get; set; }
}