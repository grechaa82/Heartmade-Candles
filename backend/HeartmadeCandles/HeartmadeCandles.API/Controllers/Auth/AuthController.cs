using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.Auth.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HeartmadeCandles.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class AuthController : Controller
{
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _userService;

    public AuthController(IOptions<JwtOptions> jwtOptions, IAuthService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var isValidUser = _userService.IsValidUser(request.Login, request.Password);

        if (isValidUser)
        {
            var token = GenerateJwtToken();

            SetTokenInCookie(token);

            return Ok(new { token });
        }

        _logger.LogError("User could not login");
        return Unauthorized();
    }

    private void SetTokenInCookie(string token)
    {
        Response.Cookies.Append(
            "token", token, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1),
                Path = "/",
                HttpOnly = true,
                Secure = true
            });
    }

    private string GenerateJwtToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new(ClaimTypes.Role, "Admin")
                }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}