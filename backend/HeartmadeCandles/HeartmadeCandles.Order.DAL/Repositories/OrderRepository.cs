using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using MongoDB.Driver;
using HeartmadeCandles.Order.DAL.Collections;
using HeartmadeCandles.Order.DAL.Mapping;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HeartmadeCandles.Order.DAL.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderCollection> _orderCollection;
    private readonly IMongoCollection<OrderDetailCollection> _orderDetailCollection;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(IMongoDatabase mongoDatabase, ILogger<OrderRepository> logger)
    {
        _orderCollection = mongoDatabase.GetCollection<OrderCollection>("order");
        _orderDetailCollection = mongoDatabase.GetCollection<OrderDetailCollection>("orderDetail");
        _logger = logger;
    }

    public async Task<Result<OrderDetail>> GetOrderDetailById(string orderDetailId)
    {
        var orderDetailCollection = await _orderDetailCollection
            .Find(x => x.Id == orderDetailId)
            .FirstOrDefaultAsync();

        var orderDetail = OrderDetailMapping.MapToOrderDetail(orderDetailCollection);

        return Result.Success(orderDetail);
    }

    public async Task<Result<string>> CreateOrderDetail(OrderDetail orderDetail)
    {
        var orderDetailItemsCollection = OrderDetailItemMapping.MapToOrderDetailItemCollection(orderDetail.Items);

        var orderDetailCollection = new OrderDetailCollection()
        {
            Items = orderDetailItemsCollection,
            TotalPrice = orderDetail.TotalPrice,
            TotalQuantity = orderDetail.TotalQuantity,
            TotalConfigurationString = orderDetail.TotalConfigurationString
        };

        await _orderDetailCollection.InsertOneAsync(orderDetailCollection);
        return Result.Success(orderDetailCollection.Id);
    }

    public async Task<Result<Core.Models.Order>> GetOrderById(string orderId)
    {
        var orderCollection = await _orderCollection
            .Find(x => x.Id == orderId)
            .FirstOrDefaultAsync();

        if (orderCollection == null)
        {
            return Result.Failure<Core.Models.Order>("OrderV2 does not exist");
        }

        var orderDetailCollection = await _orderDetailCollection
            .Find(x => x.Id == orderCollection.OrderDetailId)
            .FirstOrDefaultAsync();

        var order = OrderMapping.MapToOrder(orderCollection, orderDetailCollection);

        var json = JsonSerializer.Serialize(order);
        _logger.LogError($"OrderV2 {json}");

        return Result.Success(order);
    }

    public async Task<Result<string>> CreateOrder(Core.Models.Order order)
    {
        var json = JsonSerializer.Serialize(order);
        _logger.LogError($"OrderV2 {json}");

        var orderCollection = OrderMapping.MapToOrderCollection(order);

        await _orderCollection.InsertOneAsync(orderCollection);

        return Result.Success(orderCollection.Id);
    }
}