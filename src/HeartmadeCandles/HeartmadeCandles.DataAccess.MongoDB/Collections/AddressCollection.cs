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
        public string Country { get; set; } = string.Empty;
        
        [BsonElement("cities")]
        public string Cities { get; set; } = string.Empty;

        [BsonElement("street")]
        public string Street { get; set; } = string.Empty;

        [BsonElement("house")]
        public string House { get; set; } = string.Empty;

        [BsonElement("flat")]
        public string Flat { get; set; } = string.Empty;

        [BsonElement("index")]
        public string Index { get; set; } = string.Empty;
    }
}
