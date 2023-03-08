using HeartmadeCandles.Core.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class OrderCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customer")]
        public Customer? Customer { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("numberOfLayers")]
        public int? NumberOfLayers { get; set; }

        [BsonElement("layerColors")]
        public IDictionary<string, LayerColorCollection>? LayerColors { get; set; }

        [BsonElement("smell")]
        public SmellCollection? Smell { get; set; }

        [BsonElement("decor")]
        public DecorCollection? Decor { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}
