using HeartmadeCandles.Order.Core.Models;
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

    [BsonElement("feedback")]
    public required FeedbackDocument Feedback { get; set; }

    [BsonElement("status")]
    public OrderStatus Status { get; set; }

    [BsonElement("createdAt")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime UpdatedAt { get; set; }
}