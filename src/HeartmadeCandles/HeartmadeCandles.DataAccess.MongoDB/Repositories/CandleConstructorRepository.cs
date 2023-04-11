using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class CandleConstructorRepository : ICandleConstructorRepository
    {
        private readonly IMongoCollection<CandleCollection> _candleCollection;
        private readonly IMongoCollection<OrderCollection> _orderCollection;
        private readonly IMapper _mapper;

        public CandleConstructorRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _candleCollection = mongoDatabase.GetCollection<CandleCollection>("Candle");
            _orderCollection = mongoDatabase.GetCollection<OrderCollection>("Order");
            _mapper = mapper;
        }

        public async Task<List<Candle>> GetAllAsync()
        {
            List<BsonDocument> BsonDocumentCandles = await _candleCollection.Aggregate()
                .Lookup("LayerColor", "layerColors", "_id", "layerColors")
                .Lookup("Smell", "smells", "_id", "smells")
                .Lookup("Decor", "decors", "_id", "decors")
                .ToListAsync();

            var candles = new List<CandleCollection>();

            foreach (var item in BsonDocumentCandles)
            {
                var candle = BsonSerializer.Deserialize<CandleCollection>(item);

                candles.Add(candle);
            }

            return _mapper.Map<List<CandleCollection>, List<Candle>>(candles);
        }

        public async Task<Candle> GetByIdAsync(string id)
        {
            var BsonDocumentCandles = await _candleCollection
                .Aggregate()
                .Lookup("LayerColor", "layerColors", "_id", "layerColors")
                .Lookup("Smell", "smells", "_id", "smells")
                .Lookup("Decor", "decors", "_id", "decors")
                //.FirstOrDefaultAsync(_ => _.Id == id);
                .ToListAsync();

            CandleCollection candle;

            foreach (var item in BsonDocumentCandles)
            {
                candle = BsonSerializer.Deserialize<CandleCollection>(item);

                if (candle.Id == id)
                {
                    return _mapper.Map<CandleCollection, Candle>(candle);
                }
            }

            return default;
        }

        public async Task CreateOrder(Order order)
        {
            var orderCollection = _mapper.Map<Order, OrderCollection>(order);
            await _orderCollection.InsertOneAsync(orderCollection);
        }
    }
}
