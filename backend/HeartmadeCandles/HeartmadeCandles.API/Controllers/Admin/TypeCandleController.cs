using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.API.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Admin;

[ApiController]
[Route("api/admin/typeCandles")]
[Authorize(Roles = "Admin")]
public class TypeCandleController : Controller
{
    private readonly ILogger<TypeCandleController> _logger;
    private readonly ITypeCandleService _typeCandleService;

    public TypeCandleController(ITypeCandleService typeCandleService, ILogger<TypeCandleController> logger)
    {
        _typeCandleService = typeCandleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var typeCandlesMaybe = await _typeCandleService.GetAll();

        return Ok(typeCandlesMaybe.HasValue ? typeCandlesMaybe.Value : Array.Empty<Decor>());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var typeCandleMaybe = await _typeCandleService.Get(id);

        if (!typeCandleMaybe.HasValue)
        {
            return NotFound($"TypeCandle by id: {id} does not exist");
        }

        return Ok(typeCandleMaybe.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TypeCandleRequest typeCandleRequest)
    {
        var typeCandleResult = TypeCandle.Create(typeCandleRequest.Title, typeCandleRequest.Id);

        if (typeCandleResult.IsFailure)
        {
            return BadRequest($"Failed to create {typeof(TypeCandle)}, error message: {typeCandleResult.Error}");
        }

        var result = await _typeCandleService.Create(typeCandleResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_typeCandleService.Create),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_typeCandleService.Create)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TypeCandleRequest typeCandleRequest)
    {
        var typeCandleResult = TypeCandle.Create(typeCandleRequest.Title, id);

        if (typeCandleResult.IsFailure)
        {
            return BadRequest($"Failed to update {typeof(TypeCandle)}, error message: {typeCandleResult.Error}");
        }

        var result = await _typeCandleService.Update(typeCandleResult.Value);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_typeCandleService.Update),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_typeCandleService.Update)}, error message: {result.Error}");
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _typeCandleService.Delete(id);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_typeCandleService.Delete),
                result.Error);
            return BadRequest(
                $"Error: Failed in process {nameof(_typeCandleService.Delete)}, error message: {result.Error}");
        }

        return Ok();
    }
}