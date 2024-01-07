using Bogus;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Order.BL.Services;
using HeartmadeCandles.Order.Core.Interfaces;
using Moq;

namespace HeartmadeCandles.UnitTests.Order.BL.Services;

public class OrderServiceTest
{
    private static readonly Faker _faker = new();
    private readonly Mock<IOrderNotificationHandler> _orderNotificationHandler = new(MockBehavior.Strict);
    private readonly Mock<IOrderRepository> _orderRepository = new(MockBehavior.Strict);
    private readonly Mock<IBasketRepository> _basketRepository = new(MockBehavior.Strict);
    private readonly Mock<IConstructorService> _constructorService = new(MockBehavior.Strict);
    private readonly OrderService _service;

    public OrderServiceTest()
    {
        _service = new OrderService(
            _orderRepository.Object, 
            _basketRepository.Object, 
            _orderNotificationHandler.Object, 
            _constructorService.Object);
    }

    [Fact]
    public async Task GetOrderById_ValidId_ReturnValidOrder()
    {
        // Arrange
        var orderId = GenerateData.GenerateStringId();
        var order = GenerateOrderData.GenerateOrder();

        _orderRepository.Setup(br => br.GetOrderById(orderId))
            .ReturnsAsync(order)
            .Verifiable();

        // Act
        var result = await _service.GetOrderById(orderId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(order, result.Value);
        _orderRepository.Verify();
    }

    [Fact]
    public async Task GetOrderById_InvalidId_ReturnFailure()
    {
        // Arrange
        var orderId = GenerateData.GenerateStringId();

        _orderRepository.Setup(br => br.GetOrderById(orderId))
            .ReturnsAsync(Maybe<HeartmadeCandles.Order.Core.Models.Order>.None)
            .Verifiable();

        // Act
        var result = await _service.GetOrderById(orderId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"Order by id: {orderId} does not exist", result.Error);
        _orderRepository.Verify();
    }
}