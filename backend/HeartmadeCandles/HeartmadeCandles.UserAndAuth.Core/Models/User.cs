using CSharpFunctionalExtensions;

namespace HeartmadeCandles.UserAndAuth.Core.Models;

public class User
{
    public const int MaxUserNameLength = 48;

    private User(
        int id,
        string email,
        string userName,
        string passwordHash,
        DateTime registrationDate)
    {
        Id = id;
        Email = email;
        UserName = userName;
        PasswordHash = passwordHash;
        RegistrationDate = registrationDate;
    }

    public int Id { get; }

    public string Email { get; }

    public string UserName { get; }

    public string PasswordHash { get; }

    public DateTime RegistrationDate { get; }

    public static Result<User> Create(
        string email,
        string passwordHash,
        int id = 0,
        string? userName = null,
        DateTime? registrationDate = null)
    {
        var result = Result.Success();

        if (string.IsNullOrWhiteSpace(email))
        {
            result = Result.Combine(
                result,
                Result.Failure<User>($"'{nameof(email)}' is incorrect"));
        }

        if (string.IsNullOrWhiteSpace(userName))
        {
            var atIndex = email.IndexOf('@');
            userName = atIndex != -1 ? email.Substring(0, atIndex) : email;

            if (userName.Length > MaxUserNameLength)
            {
                userName = userName.Substring(0, MaxUserNameLength);
            }
        }
        else
        {
            if (userName.Length > MaxUserNameLength)
            {
                return Result.Failure<User>($"'{nameof(userName)}' cannot be more than {MaxUserNameLength} characters");
            }
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            result = Result.Combine(
                result,
                Result.Failure<User>($"'{nameof(passwordHash)}' cannot be null or whitespace"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<User>(result.Error);
        }

        var user = new User(
            id,
            email,
            userName,
            passwordHash,
            registrationDate ?? DateTime.UtcNow);

        return Result.Success(user);
    }
}
