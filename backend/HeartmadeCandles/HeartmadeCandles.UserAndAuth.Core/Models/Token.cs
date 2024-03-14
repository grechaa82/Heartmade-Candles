using CSharpFunctionalExtensions;

namespace HeartmadeCandles.UserAndAuth.Core.Models;

public class Token
{
    private Token(
        int id, 
        int userId, 
        string accessToken,
        string refreshToken, 
        DateTime expireTime)
    {
        Id = id;
        UserId = userId;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpireTime = expireTime;
    }

    public int Id { get; }

    public int UserId { get; }

    public string AccessToken { get; }

    public string RefreshToken { get; }

    public DateTime ExpireTime { get; }

    public static Result<Token> Create(
        int userId,
        string accessToken,
        string refreshToken,
        int id = 0,
        DateTime? expireTime = null)
    {
        var result = Result.Success();

        if (userId == 0)
        {
            result = Result.Combine(
                result,
                Result.Failure<Token>($"'{nameof(userId)}' cannot be zero"));
        }

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            result = Result.Combine(
                result,
                Result.Failure<Token>($"'{nameof(accessToken)}' cannot be null or whitespace"));
        }

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            result = Result.Combine(
                result,
                Result.Failure<Token>($"'{nameof(refreshToken)}' cannot be null or whitespace"));
        }

        if (result.IsFailure)
        {
            return Result.Failure<Token>(result.Error);
        }

        var token = new Token(
            id,
            userId,
            accessToken,
            refreshToken,
            expireTime ?? DateTime.UtcNow);

        return Result.Success(token);
    }
}
