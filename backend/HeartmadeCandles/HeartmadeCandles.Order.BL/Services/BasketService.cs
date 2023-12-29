using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Order.BL.Services;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IConstructorService _constructorService;
    private readonly ICalculateService _calculateService;

    public BasketService(
        IBasketRepository basketRepository, 
        IConstructorService constructorService,
        ICalculateService calculateService)
    {
        _basketRepository = basketRepository;
        _constructorService = constructorService;
        _calculateService = calculateService;
    }

    public async Task<Result<Basket>> GetBasketById(string basketId)
    {
        var basketMaybe = await _basketRepository.GetBasketById(basketId);
        
        return basketMaybe.HasValue
            ? Result.Success(basketMaybe.Value)
            : Result.Failure<Basket>($"Basket by id: {basketId} does not exist");
    }

    public async Task<Result<string>> CreateBasket(ConfiguredCandleBasket configuredCandleBasket)
    {
        var basketItems = new List<BasketItem>();

        foreach (var configuredCandleFilter in configuredCandleBasket.ConfiguredCandleFilters)
        {
            Result<Constructor.Core.Models.CandleDetail> candleDetail = await _constructorService.GetCandleByFilter(OrderMapping.MapToCandleDetailFilter(configuredCandleFilter));

            var configuredCandle = OrderMapping.MapConstructorCandleDetailToOrderConfiguredCandle(candleDetail.Value);

            var price = _calculateService.CalculatePrice(configuredCandle);

            var basketItemResult = BasketItem.Create(
                configuredCandle: configuredCandle,
                price: price.Value,
                configuredCandleFilter: configuredCandleFilter
            );

            if (basketItemResult.IsFailure)
            {
                return Result.Failure<string>(basketItemResult.Error);
            }

            basketItems.Add(basketItemResult.Value);
        }

        var basket = new Basket
        {
            Items = basketItems.ToArray(),
            FilterString = configuredCandleBasket.ConfiguredCandleFiltersString
        };

        var basketIdResult = await _basketRepository.CreateBasket(basket);

        return basketIdResult;
    }
}
