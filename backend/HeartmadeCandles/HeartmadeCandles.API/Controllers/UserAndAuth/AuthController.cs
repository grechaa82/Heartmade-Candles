using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HeartmadeCandles.API.Contracts.UserAndAuth.Requests;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
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
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IOptions<JwtOptions> jwtOptions, 
        IAuthService authService, 
        IUserService userService,
        IPasswordHasher passwordHasher,
        ILogger<AuthController> logger)
    {
        _jwtOptions = jwtOptions.Value;
        _authService = authService;
        _userService = userService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var userMaybe = await _userService.GetByEmail(loginRequest.Email);

        if (!userMaybe.HasValue)
        {
            _logger.LogError("");

            return BadRequest();
        }

        var isValidPassword = _passwordHasher.Verify(loginRequest.Password, userMaybe.Value.PasswordHash);

        if (!isValidPassword)
        {
            _logger.LogError("Password does not match");

            return Unauthorized();
        }

        var token = await _authService.CreateToken(userMaybe.Value);

        var tokenString = GenerateJwtToken();

        SetTokenInCookie(tokenString);

        return Ok(new { token });
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequest tokenRefreshRequest)
    {
        return Ok();
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