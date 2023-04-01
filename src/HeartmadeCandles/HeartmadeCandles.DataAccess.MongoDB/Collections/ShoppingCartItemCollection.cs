using HeartmadeCandles.Core.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class ShoppingCartItemCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; }

        [BsonElement("candle")]
        public Candle Candle { get; }

        [BsonElement("quantity")]
        public int Quantity { get; }

        [BsonElement("totalPrice")]
        public decimal TotalPrice { get; }
    }
}
