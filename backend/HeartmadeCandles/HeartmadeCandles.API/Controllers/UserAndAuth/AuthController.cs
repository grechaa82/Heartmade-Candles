using HeartmadeCandles.API.Contracts.Auth.Responses;
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
            return Unauthorized("Invalid password");
        }

        var sessionId = Guid.NewGuid();

        var tokenPayload = new TokenPayload 
        { 
            UserId = userMaybe.Value.Id, 
            UserName = userMaybe.Value.UserName, 
            Role = userMaybe.Value.Role,
            SessionId = sessionId,
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
            Id = sessionId,
            UserId = userMaybe.Value.Id,
            RefreshToken = tokenResult.Value.RefreshToken,
            ExpireAt = tokenResult.Value.ExpireAt, // TODO: Сделать время жизни RefreshToken токена 30 дней
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

        return Ok(new TokenResponse
        {
            AccessToken = tokenResult.Value.AccessToken,
            RefreshToken = tokenResult.Value.RefreshToken,
            ExpireAt = tokenResult.Value.ExpireAt
        });
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

        var tokenPayloadResult = await _tokenService.DecodeToken(tokenRequest.AccessToken);

        if (tokenPayloadResult.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_tokenService.DecodeToken),
                tokenPayloadResult.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_tokenService.DecodeToken)}, error message: {tokenPayloadResult.Error}");
        }
        
        var userMaybe = await _userService.GetById(tokenPayloadResult.Value.UserId);

        if (!userMaybe.HasValue)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_userService.GetById),
                "User not found");
            return NotFound(
                $"Error: Failed in process {nameof(_userService.GetById)}, error message: User not found");
        }

        var sessionResult = await _sessionService.GetById(tokenPayloadResult.Value.SessionId);

        if (!sessionResult.HasValue)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_sessionService.GetById),
                "Session not found");
            return NotFound(
                $"Error: Failed in process {nameof(_sessionService.GetById)}, error message: Session not found");
        }
        
        if (sessionResult.Value.RefreshToken != tokenRequest.RefreshToken 
            || sessionResult.Value.ExpireAt < DateTime.UtcNow)
        { 
            return Unauthorized("Session is incorrect");
        }

        var newTokenResult = await _tokenService.CreateToken(tokenPayloadResult.Value);

        if (newTokenResult.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_tokenService.CreateToken),
                newTokenResult.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_tokenService.CreateToken)}, error message: {newTokenResult.Error}");
        }

        var newSession = new Session
        {
            Id = sessionResult.Value.Id,
            UserId = sessionResult.Value.UserId,
            RefreshToken = newTokenResult.Value.RefreshToken,
            ExpireAt = newTokenResult.Value.ExpireAt // TODO: Сделать время жизни RefreshToken токена 30 дней
        };
        
        var newSessionResult = await _sessionService.Update(newSession);

        if (newSessionResult.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_sessionService.Update),
                newSessionResult.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_sessionService.Update)}, error message: {newSessionResult.Error}");
        }

        return Ok(new TokenResponse
        {
            AccessToken = newTokenResult.Value.AccessToken,
            RefreshToken = newTokenResult.Value.RefreshToken,
            ExpireAt = newTokenResult.Value.ExpireAt
        });
    }
}