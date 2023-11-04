using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.Order.DAL.Documents;

public class BasketDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required BasketItemDocument[] Items { get; set; }

    public decimal TotalPrice { get; set; }

    public int TotalQuantity { get; set; }
}
