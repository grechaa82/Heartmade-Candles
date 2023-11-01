namespace HeartmadeCandles.Order.Core.Models;

public class User
{
    public User(
        string firstName,
        string lastName,
        string phone,
        string? email)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Phone { get; private set; }

    public string? Email { get; private set; }
}