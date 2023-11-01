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

    [HttpGet("details/{orderDetailId}")]
    public async Task<IActionResult> GetOrderDetailById(string orderDetailId)
    {
        var result = await _orderService.GetOrderDetailById(orderDetailId);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("details/")]
    public async Task<IActionResult> CreateOrderDetail(OrderDetailItemRequest[] orderDetailItems)
    {
        var result = await _orderService.CreateOrderDetail(MapToOrderDetailItem(orderDetailItems));

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
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

    private OrderDetailItem[] MapToOrderDetailItem(OrderDetailItemRequest[] items)
    {
        List<OrderDetailItem> orderDetailItems = new List<OrderDetailItem>();

        foreach (var item in items)
        {
            OrderDetailItem orderDetailItem = new OrderDetailItem
            {
                Candle = new Candle(
                    item.Candle.Id,
                    item.Candle.Title,
                    item.Candle.Price,
                    item.Candle.WeightGrams,
                    item.Candle.Images.Select(i => new Image(i.FileName, i.AlternativeName)).ToArray(),
                    new TypeCandle(item.Candle.TypeCandle.Id, item.Candle.TypeCandle.Title)),
                Decor = item.Decor == null 
                    ? null
                    : new Decor(
                        item.Decor.Id,
                        item.Decor.Title,
                        item.Decor.Price),
                LayerColors = item.LayerColors.Select(lc => new LayerColor(
                    lc.Id,
                    lc.Title,
                    lc.PricePerGram)).ToArray(),
                NumberOfLayer = new NumberOfLayer(item.NumberOfLayer.Id, item.NumberOfLayer.Number),
                Smell = item.Smell == null 
                    ? null
                    : new Smell(item.Smell.Id, item.Smell.Title, item.Smell.Price),
                Wick = new Wick(item.Wick.Id, item.Wick.Title, item.Wick.Price),
                Quantity = item.Quantity,
                ConfigurationString = item.ConfigurationString
            };

            orderDetailItems.Add(orderDetailItem);
        }

        return orderDetailItems.ToArray();
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