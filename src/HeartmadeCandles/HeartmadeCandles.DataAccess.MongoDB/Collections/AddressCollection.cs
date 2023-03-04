using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class AddressCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("cities")]
        public string? Cities { get; set; }

        [BsonElement("street")]
        public string? Street { get; set; }

        [BsonElement("house")]
        public string? House { get; set; }

        [BsonElement("flat")]
        public string? Flat { get; set; }

        [BsonElement("index")]
        public string? Index { get; set; }
    }
}
