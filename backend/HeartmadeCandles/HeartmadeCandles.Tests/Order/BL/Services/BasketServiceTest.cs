using Bogus;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Order.BL.Services;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Moq;

namespace HeartmadeCandles.UnitTests.Order.BL.Services;

public class BasketServiceTest
{
    private static readonly Faker _faker = new();
    private readonly Mock<IBasketRepository> _basketRepository = new(MockBehavior.Strict);
    private readonly Mock<IConstructorService> _constructorService = new(MockBehavior.Strict);
    private readonly Mock<ICalculateService> _calculateService = new(MockBehavior.Strict);
    private readonly BasketService _service;

    public BasketServiceTest()
    {
        _service = new BasketService(
            _basketRepository.Object,
            _constructorService.Object,
            _calculateService.Object);
    }

    [Fact]
    public async Task GetBasketById_ValidId_ReturnValidBasket()
    {
        // Arrange
        var basketId = Guid.NewGuid().ToString();
        var basket = GenerateOrderData.GenerateBasket();

        _basketRepository.Setup(br => br.GetBasketById(basketId))
            .ReturnsAsync(basket)
            .Verifiable();

        // Act
        var result = await _service.GetBasketById(basketId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(basket, result.Value);
        _basketRepository.Verify();
    }

    [Fact]
    public async Task GetBasketById_InvalidId_ReturnFailure()
    {
        // Arrange
        var basketId = "invalidId";

        _basketRepository.Setup(br => br.GetBasketById(basketId))
            .ReturnsAsync(Maybe<Basket>.None)
            .Verifiable();

        // Act
        var result = await _service.GetBasketById(basketId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"Basket by id: {basketId} does not exist", result.Error);
        _basketRepository.Verify();
    }

    [Fact]
    public async Task CreateBasket_ValidConfiguredCandleBasket_ReturnValidBasketId()
    {

    }

    [Fact]
    public async Task CreateBasket_InvalidConfiguredCandle_ReturnFailure()
    {

    }

    [Fact]
    public async Task CreateBasket_CalculatingPriceFails_ReturnFailure()
    {

    }

    [Fact]
    public async Task CreateBasket_RepositoryFailsToCreateBasket_ReturnFailure()
    {

    }

}
