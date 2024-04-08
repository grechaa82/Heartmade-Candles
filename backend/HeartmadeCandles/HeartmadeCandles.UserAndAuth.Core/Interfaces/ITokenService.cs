using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.Core.Interfaces;

public interface ITokenService
{
    Task<Result<Token>> CreateToken(TokenPayload tokenPayload);

    Task<Result<TokenPayload>> DecodeToken (string accessToken);
}