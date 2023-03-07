using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class UserCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("nickName")]
        public string NickName { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("customer")]
        public CustomerCollection? Customer { get; set; }
        [BsonElement("role")]
        public RoleCollection Role { get; set; } = RoleCollection.Customer;
    }

    public enum RoleCollection
    {
        Customer,
        Admin
    }
}