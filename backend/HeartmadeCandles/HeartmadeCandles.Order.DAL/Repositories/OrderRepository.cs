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

    public async Task<Maybe<Core.Models.Order>> GetOrderById(string orderId)
    {
        var orderDocument = await _orderCollection
            .Find(x => x.Id == orderId)
            .FirstOrDefaultAsync();

        if (orderDocument == null)
        {
            return Maybe<Core.Models.Order>.None;
        }

        var basketDocument = await _basketCollection
            .Find(x => x.Id == orderDocument.BasketId)
            .FirstOrDefaultAsync();

        var order = OrderMapping.MapToOrder(orderDocument, basketDocument);

        return order;
    }

    public async Task<Result<string>> CreateOrder(Core.Models.Order order)
    {
        var orderDocument = OrderMapping.MapToOrderDocument(order);

        await _orderCollection.InsertOneAsync(orderDocument);

        return Result.Success(orderDocument.Id);
    }
}