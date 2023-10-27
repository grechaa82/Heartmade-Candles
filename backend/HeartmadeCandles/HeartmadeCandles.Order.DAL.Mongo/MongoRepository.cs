using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HeartmadeCandles.Order.DAL.Mongo;

public class MongoRepository : IMongoRepository
{
    public Task<Result<string>> CreateOrderDetail(OrderDetailItemV2[] orderItems)
    {
        throw new NotImplementedException();
    }

    public Task<Result<OrderDetail>> GetOrderDetailById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateOrder(OrderV2 order)
    {
        throw new NotImplementedException();
    }
}
