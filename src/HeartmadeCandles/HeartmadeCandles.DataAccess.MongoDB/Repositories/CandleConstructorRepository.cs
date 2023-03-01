using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class CandleConstructorRepository : ICandleConstructorRepository
    {
        private readonly IMongoCollection<DecorCollection> _decorCollection;
        private readonly IMapper _mapper;

        public CandleConstructorRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _decorCollection = mongoDatabase.GetCollection<DecorCollection>("Decor");
            _mapper = mapper;
        }

        public async Task<List<Decor>> GetAllAsync()
        {
            var derors = await _decorCollection.Find(_ => true).ToListAsync();

            return _mapper.Map<List<DecorCollection>, List<Decor>>(derors);
        }
    }
}
