using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class LayerColorCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("isUsed")]
        public bool IsUsed { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        public string? HEX { get; set; }

        [BsonElement("imageURL")]
        public string? ImageURL { get; set; }

        [BsonElement("pricePerGram"), BsonRepresentation(BsonType.Decimal128)]
        public decimal PricePerGram { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}
