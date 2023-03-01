using HeartmadeCandles.Core.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class CandleCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("isUsed")]
        public bool IsUsed { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("imageURL")]
        public string? ImageURL { get; set; }

        [BsonElement("numberOfLayers")]
        public List<int>? NumberOfLayers { get; set; }

        [BsonElement("layerColors")]
        public List<LayerColorCollection>? LayerColors { get; set; }

        [BsonElement("smells")]
        public List<SmellCollection>? Smells { get; set; }

        [BsonElement("decors")]
        public List<DecorCollection>? Decors { get; set; }
    }
}