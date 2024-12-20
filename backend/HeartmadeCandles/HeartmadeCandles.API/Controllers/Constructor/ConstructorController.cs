﻿using HeartmadeCandles.Constructor.Core.Interfaces;
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

    [HttpGet("candles/type")]
    public async Task<IActionResult> GetCandlesByType(string typeCandle, int pageSize = 15, int pageIndex = 0)
    {
        var (result, totalCount) = await _constructorService.GetCandlesByType(typeCandle, pageSize, pageIndex);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_constructorService.GetCandlesByType),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_constructorService.GetCandlesByType)}, error message: {result.Error}");
        }

        Response.Headers.Add("X-Total-Count", totalCount.ToString());

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