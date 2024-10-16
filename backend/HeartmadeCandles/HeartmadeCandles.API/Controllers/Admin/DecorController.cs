using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/decors")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("FixedWindowPolicy")]
public class DecorController : Controller
{
    private readonly IDecorService _decorService;
    private readonly ILogger<DecorController> _logger;

    public DecorController(IDecorService decorService, ILogger<DecorController> logger)
    {
        _decorService = decorService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationSettingsRequest pagination)
    {
        var (decorsResult, totalCount) = await _decorService.GetAll(new PaginationSettings
        {
            PageSize = pagination.Limit,
            PageIndex = pagination.Index
        });

        if (decorsResult.IsFailure)
        {
            return BadRequest(decorsResult.Error);
        }

        Response.Headers.Add("X-Total-Count", totalCount.ToString());

        return Ok(decorsResult.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var decorMaybe = await _decorService.Get(id);

        if (!decorMaybe.HasValue)
        {
            return NotFound($"Decor by id: {id} does not exist");
        }

        return Ok(decorMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DecorRequest decorRequest)
    {
        var imagesResult = ImageValidator.ValidateImages(decorRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var decorResult = Decor.Create(
            decorRequest.Title,
            decorRequest.Description,
            decorRequest.Price,
            imagesResult.Value,
            decorRequest.IsActive);

        if (decorResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Decor)}, error message: {decorResult.Error}");
        }

        var result = await _decorService.Create(decorResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_decorService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_decorService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DecorRequest decorRequest)
    {
        var imagesResult = ImageValidator.ValidateImages(decorRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var decorResult = Decor.Create(
            decorRequest.Title,
            decorRequest.Description,
            decorRequest.Price,
            imagesResult.Value,
            decorRequest.IsActive,
            id);

        if (decorResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Decor)}, error message: {decorResult.Error}");
        }

        var result = await _decorService.Update(decorResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_decorService.Update),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_decorService.Update)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _decorService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_decorService.Delete),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_decorService.Delete)}, error message: {result.Error}");
        }

        return Ok();
    }
}