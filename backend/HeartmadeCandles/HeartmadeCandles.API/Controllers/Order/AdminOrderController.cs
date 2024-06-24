using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
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
    public async Task<IActionResult> GetOrders([FromQuery] OrderQueryOptionsResponse orderQueryOptionsResponse)
    {
        var queryOptions = new OrderQueryOptions
        {
            SortBy = orderQueryOptionsResponse.SortBy,
            Ascending = orderQueryOptionsResponse.Ascending,
            CreatedFrom = orderQueryOptionsResponse.CreatedFrom,
            CreatedTo = orderQueryOptionsResponse.CreatedTo,
            Status = orderQueryOptionsResponse.Status,
        };

        var (orderMaybe, totalOrders) = await _orderService.GetOrdersWithTotalOrders(queryOptions, orderQueryOptionsResponse.pageSige, orderQueryOptionsResponse.pageIndex);

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

public class OrderQueryOptionsResponse
{
    public string? SortBy { get; init; }

    public bool Ascending { get; init; } = true;

    public DateTime? CreatedFrom { get; init; }

    public DateTime? CreatedTo { get; init; }

    public OrderStatus? Status { get; init; }

    public int pageSige { get; init; } = 10;

    public int pageIndex { get; init; } = 0;
}