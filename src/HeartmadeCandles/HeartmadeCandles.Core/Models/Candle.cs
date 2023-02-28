using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.Core.Models
{
    public class Candle
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
        public List<LayerColor>? LayerColors { get; set; }

        [BsonElement("smells")]
        public List<Smell>? Smells { get; set; }

        [BsonElement("decors")]
        public List<Decor>? Decors { get; set; }
    }
}
