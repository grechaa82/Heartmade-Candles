﻿using HeartmadeCandles.Core.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HeartmadeCandles.DataAccess.MongoDB.Collections
{
    public class OrderCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("typeDelivery")]
        public string? TypeDelivery { get; set; }

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("customer")]
        public Customer? Customer { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("numberOfLayers")]
        public int? NumberOfLayers { get; set; }

        [BsonElement("layerColors"), BsonExtraElements]
        public Dictionary<int, LayerColorCollection>? LayerColors { get; set; }

        [BsonElement("smell")]
        public SmellCollection? Smell { get; set; }

        [BsonElement("decor")]
        public DecorCollection? Decor { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }
    }
}
