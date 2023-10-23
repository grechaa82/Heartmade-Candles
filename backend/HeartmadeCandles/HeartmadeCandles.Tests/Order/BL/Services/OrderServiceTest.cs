using System.Text;
using Bogus;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.BL.Services;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Moq;

namespace HeartmadeCandles.UnitTests.Order.BL.Services;

public class OrderServiceTest
{
    private static readonly Faker _faker = new();
    private readonly Mock<IOrderNotificationHandler> _orderNotificationHandler = new(MockBehavior.Strict);

    private readonly Mock<IOrderRepository> _orderRepository = new(MockBehavior.Strict);

    private readonly OrderService _service;

    public OrderServiceTest()
    {
        _service = new OrderService(_orderRepository.Object, _orderNotificationHandler.Object);
    }

    [Fact]
    public async Task Get_WhenValid_ShouldReturnValidOrderItems()
    {
        // Arrange
        var orderItemFilters = GenerateOrderItemFilters();

        var orderItems = GenerateOrderItems(orderItemFilters, orderItemFilters.Count);

        _orderRepository.Setup(or => or.Get(orderItemFilters.ToArray()))
            .ReturnsAsync(Result.Success(orderItems.ToArray()))
            .Verifiable();

        // Act
        var result = await _service.Get(orderItemFilters.ToArray());

        // Assert
        Assert.True(result.IsSuccess);
        _orderRepository.Verify();
    }

    [Fact]
    public async Task Get_WhenOneOrderItemInvalid_ShouldReturnFailure()
    {
        // Arrange
        var orderItemFilters = GenerateOrderItemFilters();

        var invalidDecor = GenerateOrderData.GenerateDecor(_faker.Random.Number(1, 10000));
        var validOrderItem = GenerateOrderData.GenerateOrderItem(orderItemFilters[0]);
        var invalidCandleDetail = new CandleDetail(
            validOrderItem.CandleDetail.Candle, invalidDecor, validOrderItem.CandleDetail.LayerColors,
            validOrderItem.CandleDetail.NumberOfLayer, validOrderItem.CandleDetail.Smell,
            validOrderItem.CandleDetail.Wick);
        var invalidOrderItem = OrderItem.Create(
            invalidCandleDetail, validOrderItem.Quantity, validOrderItem.OrderItemFilter);

        var orderItems = GenerateOrderItems(orderItemFilters, orderItemFilters.Count - 1);
        orderItems.Add(invalidOrderItem.Value);

        _orderRepository.Setup(or => or.Get(orderItemFilters.ToArray()))
            .ReturnsAsync(Result.Success(orderItems.ToArray()))
            .Verifiable();

        // Act
        var result = await _service.Get(orderItemFilters.ToArray());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(
            result.Error,
            $"Decor by id: {validOrderItem.CandleDetail.Decor?.Id} does not match with decor by id: {invalidOrderItem.Value.CandleDetail.Decor?.Id}");
        _orderRepository.Verify();
    }

    [Fact]
    public async Task Get_WhenAllOrderItemInvalid_ShouldReturnFailure()
    {
        // Arrange
        var orderItemFilters = GenerateOrderItemFilters();

        var orderItems = new List<OrderItem>();
        var validDecorIds = new List<int>();
        var invalidDecorIds = new List<int>();

        for (var i = 0; i < orderItemFilters.Count; i++)
        {
            var invalidDecor = GenerateOrderData.GenerateDecor(_faker.Random.Number(1, 10000));
            var validOrderItem = GenerateOrderData.GenerateOrderItem(orderItemFilters[0]);
            var invalidCandleDetail = new CandleDetail(
                validOrderItem.CandleDetail.Candle, invalidDecor, validOrderItem.CandleDetail.LayerColors,
                validOrderItem.CandleDetail.NumberOfLayer, validOrderItem.CandleDetail.Smell,
                validOrderItem.CandleDetail.Wick);
            var invalidOrderItem = OrderItem.Create(
                invalidCandleDetail, validOrderItem.Quantity, validOrderItem.OrderItemFilter);

            validDecorIds.Add(validOrderItem.CandleDetail.Decor?.Id ?? 0);
            invalidDecorIds.Add(invalidOrderItem.Value.CandleDetail.Decor?.Id ?? 0);

            orderItems.Add(invalidOrderItem.Value);
        }

        _orderRepository.Setup(or => or.Get(orderItemFilters.ToArray()))
            .ReturnsAsync(Result.Success(orderItems.ToArray()))
            .Verifiable();

        var errorMessage = new StringBuilder();
        for (var i = 0; i < validDecorIds.Count && i < invalidDecorIds.Count; i++)
        {
            if (i > 0)
            {
                errorMessage.Append(", ");
            }

            errorMessage.Append(
                $"Decor by id: {validDecorIds[i]} does not match with decor by id: {invalidDecorIds[i]}");
        }

        // Act
        var result = await _service.Get(orderItemFilters.ToArray());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(errorMessage.ToString(), result.Error);
        _orderRepository.Verify();
    }

    private List<OrderItemFilter> GenerateOrderItemFilters()
    {
        var orderItemFilters = new List<OrderItemFilter>();
        for (var i = 0; i < _faker.Random.Number(1, 100); i++)
            orderItemFilters.Add(GenerateOrderData.GenerateOrderItemFilter());
        return orderItemFilters;
    }

    private List<OrderItem> GenerateOrderItems(List<OrderItemFilter> orderItemFilters, int count)
    {
        var orderItems = new List<OrderItem>();
        for (var i = 0; i < count; i++) orderItems.Add(GenerateOrderData.GenerateOrderItem(orderItemFilters[i]));
        return orderItems;
    }
}