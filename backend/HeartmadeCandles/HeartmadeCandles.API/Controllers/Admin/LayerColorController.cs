using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/layerColors")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("FixedWindowPolicy")]
public class LayerColorController : Controller
{
    private readonly ILayerColorService _layerColorService;
    private readonly ILogger<LayerColorController> _logger;

    public LayerColorController(ILayerColorService layerColorService, ILogger<LayerColorController> logger)
    {
        _layerColorService = layerColorService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var layerColorsMaybe = await _layerColorService.GetAll();

        return Ok(layerColorsMaybe.HasValue ? layerColorsMaybe.Value : Array.Empty<LayerColor>());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var layerColorMaybe = await _layerColorService.Get(id);

        if (!layerColorMaybe.HasValue)
        {
            return NotFound($"LayerColor by id: {id} does not exist");
        }

        return Ok(layerColorMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LayerColorRequest layerColorRequest)
    {
        var imagesResult = ImageValidator.ValidateImages(layerColorRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var layerColorResult = LayerColor.Create(
            layerColorRequest.Title,
            layerColorRequest.Description,
            layerColorRequest.PricePerGram,
            imagesResult.Value,
            layerColorRequest.IsActive);

        if (layerColorResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(LayerColor)}, error message: {layerColorResult.Error}");
        }

        var result = await _layerColorService.Create(layerColorResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_layerColorService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_layerColorService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, LayerColorRequest layerColorRequest)
    {
        var imagesResult = ImageValidator.ValidateImages(layerColorRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var layerColorResult = LayerColor.Create(
            layerColorRequest.Title,
            layerColorRequest.Description,
            layerColorRequest.PricePerGram,
            imagesResult.Value,
            layerColorRequest.IsActive,
            id);

        if (layerColorResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(LayerColor)}, error message: {layerColorResult.Error}");
        }

        var result = await _layerColorService.Update(layerColorResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_layerColorService.Update),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_layerColorService.Update)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _layerColorService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_layerColorService.Delete),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_layerColorService.Delete)}, error message: {result.Error}");
        }

        return Ok();
    }
}