using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HeartmadeCandles.UserAndAuth.BL.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<Token>> CreateToken(TokenPayload tokenPayload)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationOfAccessTokenInMinutes);

        var claims = GenerateClaims(
            tokenPayload.UserId, 
            tokenPayload.UserName, 
            tokenPayload.SessionId, 
            tokenPayload.Role);

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
        try
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(accessToken))
            {
                return Result.Failure<TokenPayload>("Token is incorrect");
            }

            var jsonToken = handler.ReadJwtToken(accessToken);

            var tokenPayloadJson = jsonToken.Payload.SerializeToJson();

            var tokenPayload = JsonSerializer.Deserialize<TokenPayload>(tokenPayloadJson);

            if (tokenPayload == null)
            {
                return Result.Failure<TokenPayload>("Token deserialization error");
            }

            return Result.Success(tokenPayload);
        }
        catch (Exception e)
        {
            return Result.Failure<TokenPayload>($"Token decoding error: {e.Message}");
        }
    }

    private Claim[] GenerateClaims(int userId, string userName, Guid sessionId, Role role)
    {
        return new Claim[]
        {
            new ("userid", userId.ToString(), ClaimValueTypes.Integer),
            new ("username", userName),
            new ("role", role.ToString()),
            new ("sessionid", sessionId.ToString())
        };
    }

    private string GenerateAccessToken(Claim[] claims, DateTime expireAt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

        var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expireAt,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = signingCredentials,
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];

        using var random = RandomNumberGenerator.Create();

        random.GetBytes(randomNumbers);

        return Convert.ToBase64String(randomNumbers);
    }
}