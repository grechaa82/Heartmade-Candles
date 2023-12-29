using HeartmadeCandles.API.Contracts.Order.Requests;
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

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest orderRequest)
    {
        var result = await _orderService.CreateOrder(
            MapToUser(orderRequest.User), 
            MapToFeedback(orderRequest.Feedback), 
            orderRequest.BasketId);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_orderService.CreateOrder),
                result.Error);
            return BadRequest(result.Error);
        }

        return Ok(new IdResponse
        {
            Id = result.Value
        });
    }
    
    private User MapToUser(UserRequest item)
    {
        return new User
        {
            FirstName = item.FirstName,
            LastName = item.LastName,
            Phone = item.Phone,
            Email = item.Email
        };
    }

    private Feedback MapToFeedback(FeedbackRequest item)
    {
        return new Feedback
        {
            TypeFeedback = item.Feedback.ToString(),
            UserName = item.UserName
        };
    }
}