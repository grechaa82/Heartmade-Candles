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
            orderRequest.OrderDetailId);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
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