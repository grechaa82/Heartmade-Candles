using CSharpFunctionalExtensions;
using HeartmadeCandles.API.Contracts.UserAndAuth.Requests;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class AuthController : Controller
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISessionService _sessionService;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IPasswordHasher passwordHasher,
        ISessionService sessionService,
        ITokenService tokenService,
        IUserService userService,
        ILogger<AuthController> logger)
    {
        _passwordHasher = passwordHasher;
        _sessionService = sessionService;
        _tokenService = tokenService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var userMaybe = await _userService.GetByEmail(loginRequest.Email);
        
        if (!userMaybe.HasValue)
        {
            return NotFound($"User with email: {loginRequest.Email} not found");
        }

        var isValidPassword = _passwordHasher.Verify(loginRequest.Password, userMaybe.Value.PasswordHash);
        
        if (!isValidPassword)
        {
            return Unauthorized($"Invalid password");
        }

        var tokenPayload = new TokenPayload 
        { 
            UserId = userMaybe.Value.Id, 
            UserName = userMaybe.Value.UserName, 
            Role = userMaybe.Value.Role 
        };
        
        var tokenResult = await _tokenService.CreateToken(tokenPayload);

        if (tokenResult.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", 
                nameof(_tokenService.CreateToken),
                tokenResult.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_tokenService.CreateToken)}, error message: {tokenResult.Error}");
        }

        var session = new Session
        {
            UserId = userMaybe.Value.Id,
            RefreshToken = tokenResult.Value.RefreshToken,
            ExpireAt = tokenResult.Value.ExpireAt,
        };
        
        var sessionResult = await _sessionService.Create(session);
        
        if (sessionResult.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_sessionService.Create),
                sessionResult.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_sessionService.Create)}, error message: {sessionResult.Error}");
        }

        return Ok(new { tokenResult.Value });
    }

    [HttpPost("logout")]
    [Authorize]
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