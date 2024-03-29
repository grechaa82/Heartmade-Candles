using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HeartmadeCandles.API.Contracts.UserAndAuth.Requests;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISessionService _sessionService;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IOptions<JwtOptions> jwtOptions,
        IPasswordHasher passwordHasher,
        ISessionService sessionService,
        ITokenService tokenService,
        IUserService userService,
        ILogger<AuthController> logger)
    {
        _jwtOptions = jwtOptions.Value;
        _passwordHasher = passwordHasher;
        _sessionService = sessionService;
        _tokenService = tokenService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        /*
         * var userResult = await _userService.GetByEmail(loginRequest.Email);
         * 
         * var isValidPassword = await _passwordHasher.Verify(loginRequest.Password, userMaybe.Value.PasswordHash);
         * 
         * var tokenPayload = new TokenPayload { UserId = userResult.Value.Id, UserName = userResult.Value.UserName, Role = userResult.Value.Role }
         * 
         * var tokenResult = await _tokenService.CreateToken(tokenPayload);
         * 
         * var session = new Session
         * {
         *     UserId = sessionResult.Value.UserId,
         *     RefreshToken = tokenResult.Value.RefreshToken,
         *     ExpireAt = tokenResult.Value.ExpireAt,
         * }
         * 
         * var sessionResult = await _sessionService.Create(session);
         * 
         * return Ok(new { tokenResult.Value });
         */

        // var userMaybe = await _userService.GetByEmail(loginRequest.Email);
        // 
        // if (!userMaybe.HasValue)
        // {
        //     _logger.LogError("");
        // 
        //     return BadRequest();
        // }
        // 
        // var isValidPassword = _passwordHasher.Verify(loginRequest.Password, userMaybe.Value.PasswordHash);
        // 
        // if (!isValidPassword)
        // {
        //     _logger.LogError("Password does not match");
        // 
        //     return Unauthorized();
        // }
        // 
        // var token = await _authService.CreateToken(userMaybe.Value.Id, userMaybe.Value.UserName, userMaybe.Value.Role);
        // 
        // var tokenString = GenerateJwtToken();
        // 
        // SetTokenInCookie(tokenString);
        // 
        // return Ok(new { token });

        return BadRequest();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] TokenRequest tokenRequest)
    {
        return BadRequest();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        /*
         * var tokenPayloadResult = await _tokenService.DecodeToken(tokenRequest.AccessToken)
         * 
         * var userResult = await _userService.GetById(tokenPayloadResult.Value.UserId); // Можно не запрашивать так пользователя, потому что он есть в класс Session
         * 
         * var sessionResult = await _sessionService.GetByUserId(userResult.Value.Id)
         * 
         * // Нужно проверить существует ли такой пользователь
         * 
         * if (sessionResult.Value.RefreshToken != tokenRequest.RefreshToken || sessionResult.ExpireAt < DateTime.UtcNow) => return Unauthorized();
         * 
         * var newTokenResult = await _tokenService.CreateToken(tokenPayloadResult.Value);
         * 
         * var newSession = new Session
         * {
         *     Id = sessionResult.Value.Id,
         *     UserId = sessionResult.Value.UserId,
         *     RefreshToken = newTokenResult.Value.RefreshToken,
         *     ExpireAt = newTokenResult.Value.ExpireAt,
         * }
         * 
         * var newSessionResult = await _sessionService.Update(sessionResult.Value);
         * 
         * return Ok(new { newTokenResult.Value });
         */

        // var tokenInfoResult = await _authService.DecodeToken(tokenRequest.RefreshToken);
        // 
        // if (tokenInfoResult.IsFailure)
        // {
        // 
        // }
        // 
        // var tokenMaybe = await _authService.GetTokenByUserId(tokenInfoResult.Value.UserId);
        // 
        // if (!tokenMaybe.HasValue)
        // {
        // 
        // }
        // 
        // var newTokenResult = await _authService.CreateToken(
        //     userId: tokenMaybe.Value.UserId,
        //     userName: tokenInfoResult.Value.UserName,
        //     role: tokenInfoResult.Value.Role);
        // 
        // return Ok();

        return BadRequest();
    }
}