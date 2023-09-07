using CSharpFunctionalExtensions;
using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers.Order
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{configuredCandlesString}")]
        public async Task<IActionResult> Index(string configuredCandlesString)
        {
            var arrayCandleDetailIdsWithQuantity = GetSplitetConfiguredCandlesString(configuredCandlesString)
                .Select(item => ParseUrlStringToOrderItemIds(item))
                .ToArray();

            var result = await _orderService.Get(arrayCandleDetailIdsWithQuantity);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequest orderRequest)
        {
            var arrayCandleDetailIdsWithQuantity = GetSplitetConfiguredCandlesString(orderRequest.ConfiguredCandlesString)
                .Select(item => ParseUrlStringToOrderItemIds(item))
                .ToArray();

            var result = await _orderService.CreateOrder(
                orderRequest.ConfiguredCandlesString, 
                arrayCandleDetailIdsWithQuantity,
                new User(
                    orderRequest.User.FirstName, 
                    orderRequest.User.LastName, 
                    orderRequest.User.Phone,
                    orderRequest.User.Email),
                new Feedback(
                    orderRequest.Feedback.TypeFeedback, 
                    orderRequest.Feedback.UserName));

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        private string[] GetSplitetConfiguredCandlesString(string configuredCandlesString) => configuredCandlesString.Split(".");

        private CandleDetailIdsWithQuantity ParseUrlStringToOrderItemIds(string configuredCandlesString)
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

            return new CandleDetailIdsWithQuantity(
                candleId,
                decorId,
                numberOfLayerId,
                layerColorIds,
                smellId,
                wickId,
                quantity);
        }
    }
}
