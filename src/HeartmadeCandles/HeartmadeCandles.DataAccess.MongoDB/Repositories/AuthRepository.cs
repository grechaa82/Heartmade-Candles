using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMongoCollection<UserCollection> _userCollection;
        private readonly IMapper _mapper;

        public AuthRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _userCollection = mongoDatabase.GetCollection<UserCollection>("User");
            _mapper = mapper;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var users = await _userCollection.Find(_ => true).ToListAsync();
            var user = await _userCollection.Find(x => x.Email == email).FirstOrDefaultAsync();
            return _mapper.Map<UserCollection, User>(user);
        }
    }
}