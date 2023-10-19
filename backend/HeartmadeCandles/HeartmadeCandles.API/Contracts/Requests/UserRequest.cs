using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class UserRequest
{
    [Required] public required string FirstName { get; set; }

    [Required] public required string LastName { get; set; }

    [Required] public required string Phone { get; set; }

    public string? Email { get; set; }
}