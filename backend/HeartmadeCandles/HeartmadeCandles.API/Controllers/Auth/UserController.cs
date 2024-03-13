using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Auth;

[ApiController]
[Route("api/user")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class UserController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public UserController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create()
    {
        return Ok();
    }
}
