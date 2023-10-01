using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/numberOfLayers")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("FixedWindowPolicy")]
public class NumberOfLayerController : Controller
{
    private readonly ILogger<NumberOfLayerController> _logger;
    private readonly INumberOfLayerService _numberOfLayerService;

    public NumberOfLayerController(INumberOfLayerService numberOfLayerService, ILogger<NumberOfLayerController> logger)
    {
        _numberOfLayerService = numberOfLayerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var numberOfLayersMaybe = await _numberOfLayerService.GetAll();
        return Ok(numberOfLayersMaybe.HasValue ? numberOfLayersMaybe.Value : Array.Empty<NumberOfLayer>());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var numberOfLayerMaybe = await _numberOfLayerService.Get(id);

        if (!numberOfLayerMaybe.HasValue)
        {
            return NotFound($"NumberOfLayer by id: {id} does not exist");
        }

        return Ok(numberOfLayerMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NumberOfLayerRequest numberOfLayerRequest)
    {
        var numberOfLayerResult = NumberOfLayer.Create(numberOfLayerRequest.Number);

        if (numberOfLayerResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(NumberOfLayer)}, error message: {numberOfLayerResult.Error}");
        }

        var result = await _numberOfLayerService.Create(numberOfLayerResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_numberOfLayerService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_numberOfLayerService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, NumberOfLayerRequest numberOfLayerRequest)
    {
        var numberOfLayerResult = NumberOfLayer.Create(numberOfLayerRequest.Number, id);

        if (numberOfLayerResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(NumberOfLayer)}, error message: {numberOfLayerResult.Error}");
        }

        var result = await _numberOfLayerService.Update(numberOfLayerResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_numberOfLayerService.Update),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_numberOfLayerService.Update)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _numberOfLayerService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_numberOfLayerService.Delete),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_numberOfLayerService.Delete)}, error message: {result.Error}");
        }

        return Ok();
    }
}