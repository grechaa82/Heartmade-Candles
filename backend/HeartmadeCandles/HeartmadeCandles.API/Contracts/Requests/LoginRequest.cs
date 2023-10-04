using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class LoginRequest
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}