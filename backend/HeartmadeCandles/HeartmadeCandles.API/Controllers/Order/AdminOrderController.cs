using HeartmadeCandles.API.Contracts.Order.Requests;
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
    public async Task<IActionResult> GetOrders([FromQuery] OrderTableParametersRequest orderTableParametersRequest)
    {
        var queryOptions = new OrderFilterParameters
        {
            SortBy = orderTableParametersRequest.SortBy,
            Ascending = orderTableParametersRequest.Ascending,
            CreatedFrom = orderTableParametersRequest.CreatedFrom,
            CreatedTo = orderTableParametersRequest.CreatedTo,
            Status = orderTableParametersRequest.Status,
            Pagination = new PaginationSettings
            {
                PageSize = orderTableParametersRequest.pageSige,
                PageIndex = orderTableParametersRequest.pageIndex
            }
        };

        var (orderMaybe, totalOrders) = await _orderService.GetOrdersWithTotalOrders(queryOptions);

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
