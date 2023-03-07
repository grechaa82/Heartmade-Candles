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

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userCollection.Find(x => x.Email == email).FirstOrDefaultAsync();
            return _mapper.Map<UserCollection, User>(user);
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                var userCollection = _mapper.Map<User, UserCollection>(user);
                await _userCollection.InsertOneAsync(userCollection);
            }
            catch { }
        }
    }
}