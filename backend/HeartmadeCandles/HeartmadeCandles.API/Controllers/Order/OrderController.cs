using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Order;

[ApiController]
[Route("api/orders")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    #region MongoDbRegion

    [HttpPost("v2")]
    public async Task<IActionResult> MakeOrder(OrderItemRequestV2[] orderItems)
    {
        return Ok();
    }

    [HttpGet("v2/{orderDetailId:string}")]
    public async Task<IActionResult> Get(string orderDetailId)
    {
        return Ok();
    }

    [HttpPost("v2")]
    public async Task<IActionResult> Checkout(CreateOrderRequest createOrder)
    {
        return Ok();
    }

    #endregion

    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> Get(int orderId)
    {
        var result = await _orderService.Get(orderId);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_orderService.Get),
                result.Error);

            return BadRequest($"Error: Failed in process {typeof(OrderItem)}, error message: {result.Error}");
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderItemFilterRequest[] orderItemFilter)
    {
        var result = await _orderService.CreateOrder(MapToOrderItemFilter(orderItemFilter));

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_orderService.CreateOrder),
                result.Error);

            return BadRequest($"Error: Failed in process {typeof(OrderItem)}, error message: {result.Error}");
        }

        return Ok(result.Value);
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CheckoutOrder(CreateOrderRequest orderRequest)
    {
        var result = await _orderService.CheckoutOrder(
            orderRequest.ConfiguredCandlesString,
            orderRequest.OrderId,
            MapToUser(orderRequest.User),
            MapToFeedback(orderRequest.Feedback));

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_orderService.CheckoutOrder),
                result.Error);

            return BadRequest($"Failed in process {typeof(OrderItem)}, error message: {result.Error}");
        }

        return Ok(result);
    }

    private OrderItemFilter[] MapToOrderItemFilter(OrderItemFilterRequest[] items)
    {
        return items
            .Select(
                item => new OrderItemFilter(
                    item.CandleId,
                    item.DecorId,
                    item.NumberOfLayerId,
                    item.LayerColorIds,
                    item.SmellId,
                    item.WickId,
                    item.Quantity))
            .ToArray();
    }

    private User MapToUser(UserRequest item)
    {
        return new User(item.FirstName, item.LastName, item.Phone, item.Email);
    }

    private Feedback MapToFeedback(FeedbackRequest item)
    {
        return new Feedback(item.Feedback.ToString(), item.UserName);
    }
}