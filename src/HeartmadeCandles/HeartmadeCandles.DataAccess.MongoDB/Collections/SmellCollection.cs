using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class SmellCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("isUsed")]
        public bool IsUsed { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("price"), BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }
    }
}
