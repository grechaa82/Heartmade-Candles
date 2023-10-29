using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HeartmadeCandles.Order.DAL.Mongo.Collections;

public class OrderCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public required string OrderDetailId { get; set; }

    public OrderDetailCollection? OrderDetail { get; set; }

    public required UserCollection User { get; set; }

    public required FeedbackCollection Feedback { get; set; }

    public OrderStatusCollection Status { get; set; }
}

public enum OrderStatusCollection
{
    Created, Issued, Processed, Paid, Confirmed, Assembled, Sent
}