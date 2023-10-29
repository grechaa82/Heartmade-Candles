using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.Order.DAL.Mongo.Collections;

public class OrderDetailCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required OrderDetailItemCollection[] Items { get; set; }

    public decimal TotalPrice { get; set; }

    public int TotalQuantity { get; set; }

    public required string TotalConfigurationString { get; set; }
}
