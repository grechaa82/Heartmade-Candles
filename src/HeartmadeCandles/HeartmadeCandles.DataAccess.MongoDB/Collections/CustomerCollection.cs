using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class CustomerCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; } = string.Empty;

        [BsonElement("surname")]
        public string? Surname { get; set; } = string.Empty;

        [BsonElement("middleName")]
        public string? MiddleName { get; set; } = string.Empty;

        [BsonElement("email")]
        public string? Email { get; set; } = string.Empty;

        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("typeDelivery")]
        public string TypeDelivery { get; set; } = string.Empty;

        [BsonElement("address")]
        public Address? Address { get; set; }
    }
}
