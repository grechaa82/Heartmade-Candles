using HeartmadeCandles.API.Contracts.Order.Requests;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeartmadeCandles.API.Controllers.Order;

[ApiController]
[Route("api/baskets")]
[EnableRateLimiting("ConcurrencyPolicy")]
public class BasketController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public BasketController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet("{orderDetailId}")]
    public async Task<IActionResult> GetBasketById(string orderDetailId)
    {
        var result = await _orderService.GetBasketById(orderDetailId);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBasket(CandleDetailFilterRequest[] candleDetailsFiltersRequest)
    {
        var candleDetailsFilters = candleDetailsFiltersRequest.Select(
            x => new ConfiguredCandleFilter(
            {
                CandleId = x.CandleId,
                DecorId = x.DecorId,
                NumberOfLayerId = x.NumberOfLayerId,
                LayerColorIds = x.LayerColorIds,
                SmellId = x.SmellId,
                WickId = x.WickId
            }).ToArray();

        var result = await _orderService.CreateBasket(candleDetailsFilters);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    private BasketItem[] MapToOrderDetailItem(OrderDetailItemRequest[] items)
    {
        List<BasketItem> orderDetailItems = new List<BasketItem>();

        foreach (var item in items)
        {
            BasketItem orderDetailItem = new BasketItem
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
}
