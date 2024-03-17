using HeartmadeCandles.API.Contracts.UserAndAuth.Requests;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Auth;

[ApiController]
[Route("api/user")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<AuthController> _logger;

    public UserController(
        IUserService userService,
        IPasswordHasher passwordHasher,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userMaybe = await _userService.GetById(id);

        if (!userMaybe.HasValue)
        {
            return NotFound($"User by id: {id} does not exist");
        }

        return Ok(userMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest createUserRequest)
    {
        var passwordHash = _passwordHasher.Generate(createUserRequest.Password);

        var userResult = UserAndAuth.Core.Models.User.Create(
            email: createUserRequest.Email,
            passwordHash: passwordHash,
            userName: createUserRequest.UserName);

        if (userResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(User)}, error message: {userResult.Error}");
        }

        var result = await _userService.Create(userResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", 
                nameof(_userService.Create),
                result.Error);
            return BadRequest($"Error: Failed in process {nameof(_userService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_userService.Delete),
                result.Error);
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
