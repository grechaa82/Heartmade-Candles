using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<DecorCollection> _decorCollection;
        private readonly IMapper _mapper;

        public AdminRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _decorCollection = mongoDatabase.GetCollection<DecorCollection>("Decor");
            _mapper = mapper;
        }

        #region Decor

        public async Task<List<Decor>> GetDecorAsync()
        {
            var decors = await _decorCollection.Find(decor => true).ToListAsync();
            return _mapper.Map<List<DecorCollection>, List<Decor>>(decors);
        }

        public async Task CreateDecorAsync(Decor decor)
        {
            var decorCollection = _mapper.Map<Decor, DecorCollection>(decor);
            await _decorCollection.InsertOneAsync(decorCollection);
        }

        public async Task UpdateDecorAsync(Decor decor)
        {
            var decorChecker = await _decorCollection.Find(d => d.Id == decor.Id).FirstAsync();

            if (decorChecker is null)
            {
                throw new ArgumentNullException();
            }

            var decorCollection = _mapper.Map<Decor, DecorCollection>(decor);

            await _decorCollection.ReplaceOneAsync(d => d.Id == decor.Id, decorCollection);
        }

        public async Task DeleteDecorAsync(string id)
        {
            await _decorCollection.DeleteOneAsync(decor => decor.Id == id);
        }

        #endregion
    }
}