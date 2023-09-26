using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/candles")]
[Authorize(Roles = "Admin")]
public class CandleController : Controller
{
    private readonly ICandleService _candleService;
    private readonly ILogger<CandleController> _logger;

    public CandleController(ICandleService candleService,
        ILogger<CandleController> logger)
    {
        _candleService = candleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var candlesMaybe = await _candleService.GetAll();

        if (!candlesMaybe.HasValue)
        {
            return Ok(Array.Empty<Candle>());
        }

        return Ok(candlesMaybe.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var candleMaybe = await _candleService.Get(id);

        if (!candleMaybe.HasValue)
        {
            return NotFound($"Candle by id: {id} does not exist");
        }

        return Ok(candleMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CandleRequest candleRequest)
    {
        if (candleRequest.TypeCandle.Id <= 0)
        {
            return BadRequest(
                $"Unable to request {nameof(candleRequest.TypeCandle)} by id: {candleRequest.TypeCandle.Id} does not exist");
        }

        var typeCandleResult = TypeCandle.Create(
            candleRequest.TypeCandle.Title,
            candleRequest.TypeCandle.Id);

        if (typeCandleResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(TypeCandle)}, error message: {typeCandleResult.Error}");
        }

        var imagesResult = ImageValidator.ValidateImages(candleRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var candleResult = Candle.Create(
            candleRequest.Title,
            candleRequest.Description,
            candleRequest.Price,
            candleRequest.WeightGrams,
            imagesResult.Value,
            typeCandleResult.Value,
            candleRequest.IsActive);

        if (candleResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(Candle)}, error message: {candleResult.Error}");
        }

        var result = await _candleService.Create(candleResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_candleService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_candleService.Create)}, error message: {result.Error}");
        }

        _logger.LogInformation("Candle was created by name: {title}", candleRequest.Title);

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,
        CandleRequest candleRequest)
    {
        if (candleRequest.TypeCandle.Id <= 0)
        {
            return BadRequest(
                $"Unable to request {nameof(candleRequest.TypeCandle)} by id: {candleRequest.TypeCandle.Id} does not exist");
        }

        var typeCandleResult = TypeCandle.Create(
            candleRequest.TypeCandle.Title,
            candleRequest.TypeCandle.Id);

        if (typeCandleResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(TypeCandle)}, error message: {typeCandleResult.Error}");
        }

        var imagesResult = ImageValidator.ValidateImages(candleRequest.Images);

        if (imagesResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Image)}, error message: {imagesResult.Error}");
        }

        var candleResult = Candle.Create(
            candleRequest.Title,
            candleRequest.Description,
            candleRequest.Price,
            candleRequest.WeightGrams,
            imagesResult.Value,
            typeCandleResult.Value,
            candleRequest.IsActive,
            id);

        if (candleResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(Candle)}, error message: {candleResult.Error}");
        }

        var result = await _candleService.Update(candleResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_candleService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_candleService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _candleService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}", nameof(_candleService.Delete),
                result.Error);
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPut("{id:int}/decors")]
    public async Task<IActionResult> UpdateDecor(int id,
        int[] decorsIds)
    {
        var result = await _candleService.UpdateDecor(
            id,
            decorsIds);

        if (result.IsFailure)
        {
            return BadRequest(
                $"Error: Failed in process {nameof(_candleService.UpdateDecor)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}/layerColors")]
    public async Task<IActionResult> UpdateLayerColor(int id,
        int[] layerColorsIds)
    {
        var result = await _candleService.UpdateLayerColor(
            id,
            layerColorsIds);

        if (result.IsFailure)
        {
            return BadRequest(
                $"Error: Failed in process {nameof(_candleService.UpdateLayerColor)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}/numberOfLayers")]
    public async Task<IActionResult> UpdateNumberOfLayer(int id,
        int[] numberOfLayersIds)
    {
        var result = await _candleService.UpdateNumberOfLayer(
            id,
            numberOfLayersIds);

        if (result.IsFailure)
        {
            return BadRequest(
                $"Error: Failed in process {nameof(_candleService.UpdateNumberOfLayer)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}/smells")]
    public async Task<IActionResult> UpdateSmell(int id,
        int[] smellsIds)
    {
        var result = await _candleService.UpdateSmell(
            id,
            smellsIds);

        if (result.IsFailure)
        {
            return BadRequest(
                $"Error: Failed in process {nameof(_candleService.UpdateSmell)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}/wicks")]
    public async Task<IActionResult> UpdateWick(int id,
        int[] wicksIds)
    {
        var result = await _candleService.UpdateWick(
            id,
            wicksIds);

        if (result.IsFailure)
        {
            return BadRequest(
                $"Error: Failed in process {nameof(_candleService.UpdateWick)}, error message: {result.Error}");
        }

        return Ok();
    }
}