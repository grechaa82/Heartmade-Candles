using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class CandleConstructorRepository : ICandleConstructorRepository
    {
        private readonly IMongoCollection<Decor> _candleCollection;

        public CandleConstructorRepository(IMongoDatabase mongoDatabase)
        {
            _candleCollection = mongoDatabase.GetCollection<Decor>("Decor");
        }

        public async Task<List<Decor>> GetAllAsync()
        {
            return await _candleCollection.Find(_ => true).ToListAsync();
        }
    }
}
