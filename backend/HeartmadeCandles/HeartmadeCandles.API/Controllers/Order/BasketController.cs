﻿using HeartmadeCandles.API.Contracts.Order.Requests;
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
    private readonly ILogger<BasketController> _logger;
    private readonly IOrderService _orderService;

    public BasketController(IOrderService orderService, ILogger<BasketController> logger)
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
    public async Task<IActionResult> CreateBasket([FromBody] ConfiguredCandleBasketRequest configuredCandleBasketRequest)
    {
        var configuredCandlesFilters= configuredCandleBasketRequest.CandleDetailFilterRequests
            .Select(
                x => new ConfiguredCandleFilter
                {
                    CandleId = x.CandleId,
                    DecorId = x.DecorId,
                    NumberOfLayerId = x.NumberOfLayerId,
                    LayerColorIds = x.LayerColorIds,
                    SmellId = x.SmellId,
                    WickId = x.WickId,
                    Quantity = x.Quantity,
                })
            .ToArray();

        var result = await _orderService.CreateBasket(new ConfiguredCandleBasket
        {
            ConfiguredCandleFilters = configuredCandlesFilters,
            ConfiguredCandleFiltersString = configuredCandleBasketRequest.ConfiguredCandleFiltersString
        });

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new IdResponse
        {
            Id = result.Value
        });
    }
}

public class IdResponse
{
    public string Id { get; set; }
}