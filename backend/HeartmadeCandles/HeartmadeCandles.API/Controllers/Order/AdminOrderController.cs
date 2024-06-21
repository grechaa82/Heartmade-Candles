using HeartmadeCandles.Order.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Order;

[ApiController]
[Route("api/admin/orders")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class AdminOrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public AdminOrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(int pageSige = 10, int pageIndex = 0)
    {
        var (orderMaybe, totalOrders) = await _orderService.GetOrdersWithTotalOrders(pageSige, pageIndex);

        if (!orderMaybe.HasValue)
        {
            _logger.LogWarning(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_orderService.GetOrdersWithTotalOrders),
                "Orders not found");
            return NotFound();
        }

        return Ok(orderMaybe.Value);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(string orderId)
    {
        var result = await _orderService.GetOrderById(orderId);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_orderService.GetOrderById),
                result.Error);
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}