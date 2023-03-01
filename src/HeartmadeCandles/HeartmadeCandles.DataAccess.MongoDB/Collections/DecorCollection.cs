using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class DecorCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("isUsed")]
        public bool IsUsed { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("imageURL")]
        public string? ImageURL { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }
    }
}