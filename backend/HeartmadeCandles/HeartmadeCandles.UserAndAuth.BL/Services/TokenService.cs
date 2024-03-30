using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;

namespace HeartmadeCandles.UserAndAuth.BL.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IJwtAlgorithm _jwtAlgorithm = new HMACSHA256Algorithm();

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<Token>> CreateToken(TokenPayload tokenPayload)
    {
        var claims = new Dictionary<string, object> 
        {
            { nameof(tokenPayload.UserId), tokenPayload.UserId },
            { nameof(tokenPayload.UserName), tokenPayload.UserName },
            { nameof(tokenPayload.Role), tokenPayload.Role },
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes);

        var accessToken = GenerateAccessToken(claims, expiresAt);

        var refreshToken = GenerateRefreshToken();

        var token = new Token 
        { 
            AccessToken = accessToken, 
            RefreshToken = refreshToken,
            ExpireAt = expiresAt,
        };

        return token;
    }

    public async Task<Result<TokenPayload>> DecodeToken(string accessToken)
    {
        throw new NotImplementedException();
    }


    private string GenerateAccessToken(IDictionary<string, object> claims, DateTime expireAt) =>
        JwtBuilder
            .Create()
            .WithAlgorithm(_jwtAlgorithm)
            .WithSecret(_jwtOptions.SecretKey)
            .ExpirationTime(expireAt)
            .WithVerifySignature(true)
            .AddClaims(claims)
            .Encode();

    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];

        using var random = RandomNumberGenerator.Create();

        random.GetBytes(randomNumbers);

        return Convert.ToBase64String(randomNumbers);
    }
}