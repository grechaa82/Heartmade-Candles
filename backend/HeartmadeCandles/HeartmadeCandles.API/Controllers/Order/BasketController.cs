using HeartmadeCandles.API.Contracts.Order.Requests;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Order;

[ApiController]
[Route("api/baskets")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class BasketController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public BasketController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet("{orderDetailId}")]
    public async Task<IActionResult> GetBasketById(string orderDetailId)
    {
        var result = await _orderService.GetBasketById(orderDetailId);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBasket(CandleDetailFilterRequest[] candleDetailsFiltersRequest)
    {
        var configuredCandlesFilters= candleDetailsFiltersRequest
            .Select(
                x => new ConfiguredCandleFilter
                {
                    CandleId = x.CandleId,
                    DecorId = x.DecorId,
                    NumberOfLayerId = x.NumberOfLayerId,
                    LayerColorIds = x.LayerColorIds,
                    SmellId = x.SmellId,
                    WickId = x.WickId
                })
            .ToArray();

        var result = await _orderService.CreateBasket(configuredCandlesFilters);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
