using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/wicks")]
[Authorize(Roles = "Admin")]
public class WickController : Controller
{
    private readonly ILogger<WickController> _logger;
    private readonly IWickService _wickService;

    public WickController(IWickService wickService, ILogger<WickController> logger)
    {
        _wickService = wickService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var wicksMaybe = await _wickService.GetAll();

        return Ok(wicksMaybe.HasValue ? wicksMaybe.Value : Array.Empty<Decor>());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var wickMaybe = await _wickService.Get(id);

        if (!wickMaybe.HasValue)
        {
            return NotFound($"Wick by id: {id} does not exist");
        }

        return Ok(wickMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(WickRequest wickRequest)
    {
        var imagesResult = ImageValidator.ValidateImages(wickRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var wickResult = Wick.Create(
            wickRequest.Title,
            wickRequest.Description,
            wickRequest.Price,
            imagesResult.Value,
            wickRequest.IsActive);

        if (wickResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Wick)}, error message: {wickResult.Error}");
        }

        var result = await _wickService.Create(wickResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_wickService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_wickService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, WickRequest wickRequest)
    {
        var imagesResult = ImageValidator.ValidateImages(wickRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var wickResult = Wick.Create(
            wickRequest.Title,
            wickRequest.Description,
            wickRequest.Price,
            imagesResult.Value,
            wickRequest.IsActive,
            id);

        if (wickResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Wick)}, error message: {wickResult.Error}");
        }

        var result = await _wickService.Update(wickResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_wickService.Update),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_wickService.Update)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _wickService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_wickService.Delete),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_wickService.Delete)}, error message: {result.Error}");
        }

        return Ok();
    }
}