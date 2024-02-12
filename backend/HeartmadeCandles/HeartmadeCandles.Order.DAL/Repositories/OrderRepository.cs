using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using MongoDB.Driver;
using HeartmadeCandles.Order.DAL.Documents;
using HeartmadeCandles.Order.DAL.Mapping;
using MongoDB.Bson;
using HeartmadeCandles.Order.Core.Models;

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

    public async Task<Maybe<Core.Models.Order[]>> GetOrderByStatus(OrderStatus status)
    {
        var orderDocument = await _orderCollection
            .Find(x => x.Status == status)
            .ToListAsync();

        if (orderDocument.Count == 0)
        {
            return Maybe<Core.Models.Order[]>.None;
        }

        var order = orderDocument
            .Select(OrderMapping.MapToOrder)
            .ToArray();

        return order;
    }

    public async Task<Maybe<Core.Models.Order[]>> GetOrderByStatus(OrderStatus status, int pageSige, int pageIndex)
    {
        var orderDocument = await _orderCollection
            .Find(x => x.Status == status)
            .Skip(pageIndex * pageSige)
            .Limit(pageSige)
            .ToListAsync();

        if (orderDocument.Count == 0)
        {
            return Maybe<Core.Models.Order[]>.None;
        }

        var order = orderDocument
            .Select(OrderMapping.MapToOrder)
            .ToArray();

        return order;
    }

    public async Task<Result<string>> CreateOrder(Core.Models.Order order)
    {
        var orderDocument = OrderMapping.MapToOrderDocument(order);

        await _orderCollection.InsertOneAsync(orderDocument);

        return Result.Success(orderDocument.Id);
    }

    public async Task<Result> UpdateOrderStatus(string orderId, OrderStatus status)
    {
        var update = Builders<OrderDocument>.Update.Set(x => x.Status, status);

        await _orderCollection.UpdateOneAsync(x => x.Id == orderId, update: update);

        return Result.Success();
    }
}