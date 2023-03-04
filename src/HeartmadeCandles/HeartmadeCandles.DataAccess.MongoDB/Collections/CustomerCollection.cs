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
        public string? Name { get; set; }

        [BsonElement("surname")]
        public string? Surname { get; set; }

        [BsonElement("middleName")]
        public string? MiddleName { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("typeDelivery")]
        public string? TypeDelivery { get; set; }

        [BsonElement("address")]
        public Address? Address { get; set; }
    }
}
