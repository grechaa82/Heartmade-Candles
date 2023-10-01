using HeartmadeCandles.Constructor.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Constructor;

[ApiController]
[Route("api/constructor")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class ConstructorController : Controller
{
    private readonly IConstructorService _constructorService;
    private readonly ILogger<ConstructorController> _logger;

    public ConstructorController(IConstructorService constructorService, ILogger<ConstructorController> logger)
    {
        _constructorService = constructorService;
        _logger = logger;
    }

    [HttpGet("candles")]
    public async Task<IActionResult> GetCandles()
    {
        var result = await _constructorService.GetCandles();

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_constructorService.GetCandles),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_constructorService.GetCandles)}, error message: {result.Error}");
        }

        return Ok(result.Value);
    }

    [HttpGet("candles/{candleId}")]
    public async Task<IActionResult> GetCandleById(int candleId)
    {
        var result = await _constructorService.GetCandleDetailById(candleId);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_constructorService.GetCandleDetailById),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_constructorService.GetCandleDetailById)}, error message: {result.Error}");
        }

        return Ok(result.Value);
    }
}