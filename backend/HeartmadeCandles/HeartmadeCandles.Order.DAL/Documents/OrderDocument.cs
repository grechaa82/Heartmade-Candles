using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HeartmadeCandles.Order.DAL.Documents;

public class OrderDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("basketId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string BasketId { get; set; }

    [BsonElement("basket")]
    public BasketDocument? Basket { get; set; }

    [BsonElement("user")]
    public required UserDocument User { get; set; }

    [BsonElement("feedback")]
    public required FeedbackDocument Feedback { get; set; }

    [BsonElement("status")]
    public OrderStatus Status { get; set; }
}