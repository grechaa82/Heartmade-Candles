namespace HeartmadeCandles.Order.Core.Models;

public class User
{
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Phone { get; init; }

    public string? Email { get; init; }
}