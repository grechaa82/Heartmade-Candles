using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.BL.Services;

public class TokenService : ITokenService
{
    public async Task<Result<Token>> CreateToken(TokenPayload tokenPayload)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<TokenPayload>> DecodeToken(string accessToken)
    {
        throw new NotImplementedException();
    }
}