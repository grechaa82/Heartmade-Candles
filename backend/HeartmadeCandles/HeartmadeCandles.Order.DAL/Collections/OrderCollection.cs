using HeartmadeCandles.Order.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HeartmadeCandles.Order.DAL.Collections;

public class OrderCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("orderDetailId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string OrderDetailId { get; set; }

    [BsonElement("orderDetail")]
    public OrderDetailCollection? OrderDetail { get; set; }

    [BsonElement("user")]
    public required UserCollection User { get; set; }

    [BsonElement("feedback")]
    public required FeedbackCollection Feedback { get; set; }

    [BsonElement("status")]
    public OrderStatus Status { get; set; }
}