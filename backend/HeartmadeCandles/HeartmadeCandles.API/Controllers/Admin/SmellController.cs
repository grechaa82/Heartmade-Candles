using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/smells")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("FixedWindowPolicy")]
public class SmellController : Controller
{
    private readonly ILogger<SmellController> _logger;
    private readonly ISmellService _smellService;

    public SmellController(ISmellService smellService, ILogger<SmellController> logger)
    {
        _smellService = smellService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var smellsMaybe = await _smellService.GetAll();

        return Ok(smellsMaybe.HasValue ? smellsMaybe.Value : Array.Empty<Smell>());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var smellMaybe = await _smellService.Get(id);

        if (!smellMaybe.HasValue)
        {
            return NotFound($"Smell by id: {id} does not exist");
        }

        return Ok(smellMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SmellRequest smellRequest)
    {
        var smellResult = Smell.Create(
            smellRequest.Title,
            smellRequest.Description,
            smellRequest.Price,
            smellRequest.IsActive);

        if (smellResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Smell)}, error message: {smellResult.Error}");
        }

        var result = await _smellService.Create(smellResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_smellService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_smellService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SmellRequest smellRequest)
    {
        var smellResult = Smell.Create(
            smellRequest.Title,
            smellRequest.Description,
            smellRequest.Price,
            smellRequest.IsActive,
            id);

        if (smellResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Smell)}, error message: {smellResult.Error}");
        }

        var result = await _smellService.Update(smellResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_smellService.Update),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_smellService.Update)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _smellService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_smellService.Delete),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_smellService.Delete)}, error message: {result.Error}");
        }

        return Ok();
    }
}