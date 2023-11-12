using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using MongoDB.Driver;
using HeartmadeCandles.Order.DAL.Documents;
using HeartmadeCandles.Order.DAL.Mapping;

namespace HeartmadeCandles.Order.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderDocument> _orderCollection;
    private readonly IMongoCollection<BasketDocument> _basketCollection;

    public OrderRepository(IMongoDatabase mongoDatabase)
    {
        _orderCollection = mongoDatabase.GetCollection<OrderDocument>("order");
        _basketCollection = mongoDatabase.GetCollection<BasketDocument>("basket");
    }

    public async Task<Result<Basket>> GetBasketById(string basketId)
    {
        var basketDocument = await _basketCollection
            .Find(x => x.Id == basketId)
            .FirstOrDefaultAsync();

        if (basketDocument == null)
        {
            return Result.Failure<Basket>($"Basket by id: {basketId} does not exist");
        }

        var orderDetail = BasketMapping.MapToBasket(basketDocument);

        return Result.Success(orderDetail);
    }

    public async Task<Result<string>> CreateBasket(Basket basket)
    {
        var basketDocument = new BasketDocument
        {
            Items = BasketItemMapping.MapToBasketItemDocument(basket.Items),
            TotalPrice = basket.TotalPrice,
            TotalQuantity = basket.TotalQuantity,
            FilterString = basket.FilterString,
        };

        await _basketCollection.InsertOneAsync(basketDocument);

        return Result.Success(basketDocument.Id);
    }

    public async Task<Result<Core.Models.Order>> GetOrderById(string orderId)
    {
        var orderDocument = await _orderCollection
            .Find(x => x.Id == orderId)
            .FirstOrDefaultAsync();

        if (orderDocument == null)
        {
            return Result.Failure<Core.Models.Order>($"Order by id: {orderId} does not exist");
        }

        var basketDocument = await _basketCollection
            .Find(x => x.Id == orderDocument.BasketId)
            .FirstOrDefaultAsync();

        var order = OrderMapping.MapToOrder(orderDocument, basketDocument);

        return Result.Success(order);
    }

    public async Task<Result<string>> CreateOrder(Core.Models.Order order)
    {
        var orderDocument = OrderMapping.MapToOrderDocument(order);

        await _orderCollection.InsertOneAsync(orderDocument);

        return Result.Success(orderDocument.Id);
    }
}