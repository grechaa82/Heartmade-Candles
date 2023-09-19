using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Order;

[ApiController]
[Route("api/orders")]
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
        _logger.LogInformation(
            "Request {@Controller} {@Endpoint}, configuredCandlesString: {@ConfiguredCandlesString}, {@DataTime}",
            nameof(OrderController),
            nameof(Get),
            configuredCandlesString,
            DateTime.UtcNow);

        var orderItemFilters = GetSplitetConfiguredCandlesString(configuredCandlesString)
            .Select(item => ParseUrlStringToOrderItemFilter(item))
            .ToArray();

        _logger.LogInformation("orderItemFilters: {0}", orderItemFilters);

        var result = await _orderService.Get(orderItemFilters);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error request {0} {1}, {2}, {3}",
                nameof(OrderController),
                nameof(Get),
                result.Error,
                DateTime.UtcNow);

            return BadRequest(result.Error);
        }

        _logger.LogInformation(
            "When Request {0} {1}, an order is received: {2}, configuredCandlesString: {3}, {4}",
            nameof(OrderController),
            nameof(Get),
            result.Value,
            configuredCandlesString,
            DateTime.UtcNow);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest orderRequest)
    {
        _logger.LogInformation(
            "Request {0} {1}, orderRequest: {2}, {3}",
            nameof(OrderController),
            nameof(CreateOrder),
            orderRequest,
            DateTime.UtcNow);

        var result = await _orderService.CreateOrder(
            orderRequest.ConfiguredCandlesString,
            MapToOrderItemFilter(orderRequest.OrderItemFilters),
            MapToUser(orderRequest.User),
            MapToFeedback(orderRequest.Feedback));

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error request {0} {1}, {2}, {3}",
                nameof(OrderController),
                nameof(CreateOrder),
                result.Error,
                DateTime.UtcNow);

            return BadRequest(result.Error);
        }

        _logger.LogInformation(
            "When Request {0} {1}, order has been created, order: {2}, {3}",
            nameof(OrderController),
            nameof(CreateOrder),
            result,
            DateTime.UtcNow);

        return Ok(result);
    }

    private string[] GetSplitetConfiguredCandlesString(string configuredCandlesString)
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
                return values.Select(v => int.Parse(v)).ToArray();
            }

            return null;
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