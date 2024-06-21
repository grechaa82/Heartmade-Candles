using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using MongoDB.Driver;
using HeartmadeCandles.Order.DAL.Documents;
using HeartmadeCandles.Order.DAL.Mapping;
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

    public async Task<(Maybe<Core.Models.Order[]>, long)> GetOrdersWithTotalOrders(int pageSige, int pageIndex)
    {
        var totalOrders = await _orderCollection.EstimatedDocumentCountAsync();

        var orderDocument = await _orderCollection
            .Find(_ => true)
            .Skip(pageIndex * pageSige)
            .Limit(pageSige)
            .ToListAsync();

        if (orderDocument.Count == 0)
        {
            return (Maybe<Core.Models.Order[]>.None, totalOrders);
        }

        var order = orderDocument
            .Select(OrderMapping.MapToOrder)
            .ToArray();

        return (order, totalOrders);
    }

    public async Task<(Maybe<Core.Models.Order[]>, long)> GetOrderByStatusWithTotalOrders(OrderStatus status, int pageSige, int pageIndex)
    {
        var totalOrders = await _orderCollection.CountDocumentsAsync(x => x.Status == status);

        var orderDocument = await _orderCollection
            .Find(x => x.Status == status)
            .Skip(pageIndex * pageSige)
            .Limit(pageSige)
            .ToListAsync();

        if (orderDocument.Count == 0)
        {
            return (Maybe<Core.Models.Order[]>.None, totalOrders);
        }

        var order = orderDocument
            .Select(OrderMapping.MapToOrder)
            .ToArray();

        return (order, totalOrders);
    }

    public async Task<Result<string>> CreateOrder(Core.Models.Order order)
    {
        var orderDocument = OrderMapping.MapToOrderDocument(order);

        await _orderCollection.InsertOneAsync(orderDocument);

        return Result.Success(orderDocument.Id);
    }

    public async Task<Result> UpdateOrder(Core.Models.Order order)
    {
        var orderDocument = OrderMapping.MapToOrderDocument(order);

        var filter = Builders<OrderDocument>.Filter.Eq(x => x.Id, order.Id);
        var update = Builders<OrderDocument>.Update
            .Set(x => x.BasketId, orderDocument.BasketId)
            .Set(x => x.Basket, orderDocument.Basket)
            .Set(x => x.Feedback, orderDocument.Feedback)
            .Set(x => x.Status, orderDocument.Status)
            .Set(x => x.CreatedAt, orderDocument.CreatedAt)
            .Set(x => x.UpdatedAt, orderDocument.UpdatedAt);

        var result = await _orderCollection.UpdateOneAsync(filter, update);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return Result.Success();
        }

        return Result.Failure("Failed to update the order");
    }

    public async Task<Result> UpdateOrderStatus(string orderId, OrderStatus status)
    {
        var update = Builders<OrderDocument>.Update.Set(x => x.Status, status);

        await _orderCollection.UpdateOneAsync(x => x.Id == orderId, update: update);

        return Result.Success();
    }
}