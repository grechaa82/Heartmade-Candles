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
        private readonly IMongoCollection<SmellCollection> _smellCollection;
        private readonly IMapper _mapper;

        public AdminRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _decorCollection = mongoDatabase.GetCollection<DecorCollection>("Decor");
            _smellCollection = mongoDatabase.GetCollection<SmellCollection>("Smell");
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

        #region Smell

        public async Task<List<Smell>> GetSmellAsync()
        {
            var smells = await _smellCollection.Find(decor => true).ToListAsync();
            return _mapper.Map<List<SmellCollection>, List<Smell>>(smells);
        }

        public async Task CreateSmellAsync(Smell smell)
        {
            var decorCollection = _mapper.Map<Smell, SmellCollection>(smell);
            await _smellCollection.InsertOneAsync(decorCollection);
        }

        public async Task UpdateSmellAsync(Smell smell)
        {
            var smellChecker = await _smellCollection.Find(d => d.Id == smell.Id).FirstAsync();

            if (smellChecker is null)
            {
                throw new ArgumentNullException();
            }

            var smellCollection = _mapper.Map<Smell, SmellCollection>(smell);

            await _smellCollection.ReplaceOneAsync(d => d.Id == smell.Id, smellCollection);
        }

        public async Task DeleteSmellAsync(string id)
        {
            await _smellCollection.DeleteOneAsync(smell => smell.Id == id);
        }

        #endregion
    }
}