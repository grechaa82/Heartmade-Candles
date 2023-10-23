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

    [HttpGet("{configuredCandlesString}")]
    public async Task<IActionResult> Get(string configuredCandlesString)
    {
        var orderItemFilters = GetSplittedConfiguredCandlesString(configuredCandlesString)
            .Select(ParseUrlStringToOrderItemFilter)
            .ToArray();

        var result = await _orderService.Get(orderItemFilters);

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
    public async Task<IActionResult> CreateOrder(CreateOrderRequest orderRequest)
    {
        var result = await _orderService.CreateOrder(
            orderRequest.ConfiguredCandlesString,
            MapToOrderItemFilter(orderRequest.OrderItemFilters),
            MapToUser(orderRequest.User),
            MapToFeedback(orderRequest.Feedback));

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error: Failed in process {processName}, error message: {errorMessage}",
                nameof(_orderService.CreateOrder),
                result.Error);
            return BadRequest($"Failed in process {typeof(OrderItem)}, error message: {result.Error}");
        }

        return Ok(result);
    }

    private string[] GetSplittedConfiguredCandlesString(string configuredCandlesString)
    {
        return configuredCandlesString.Split(".");
    }

    private OrderItemFilter ParseUrlStringToOrderItemFilter(string configuredCandlesString)
    {
        var candleParts = configuredCandlesString.Split('~');
        var candleId = GetValue(candleParts, "c");
        var decorId = GetValue(candleParts, "d");
        var numberOfLayerId = GetValue(candleParts, "n");
        var layerColorIds = GetArrayValue(candleParts, "l");
        var smellId = GetValue(candleParts, "s");
        var wickId = GetValue(candleParts, "w");
        var quantity = GetValue(candleParts, "q");

        int GetValue(string[] parts, string key)
        {
            var valueString = parts.FirstOrDefault(part => part.StartsWith(key + "-"))?.Substring(key.Length + 1);

            if (valueString != null)
            {
                var value = int.Parse(valueString);
                return value;
            }

            return 0;
        }

        int[] GetArrayValue(string[] parts, string key)
        {
            var value = parts.FirstOrDefault(part => part.StartsWith(key + "-"))?.Substring(key.Length + 1);
            if (value != null)
            {
                var values = value.Split('_');
                return values.Select(int.Parse).ToArray();
            }

            return Array.Empty<int>();
        }

        return new OrderItemFilter(
            candleId,
            decorId,
            numberOfLayerId,
            layerColorIds,
            smellId,
            wickId,
            quantity);
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