using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserCollection> _userCollection;
        private readonly IMongoCollection<CustomerCollection> _customerCollection;
        private readonly IMongoCollection<AddressCollection> _addressCollection;
        private readonly IMapper _mapper;

        public UserRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _userCollection = mongoDatabase.GetCollection<UserCollection>("User");
            _customerCollection = mongoDatabase.GetCollection<CustomerCollection>("Customer");
            _addressCollection = mongoDatabase.GetCollection<AddressCollection>("Address");
            _mapper = mapper;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await _userCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<UserCollection>, List<User>>(users);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var user = await _userCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<UserCollection, User>(user);
        }

        public async Task<Customer> GetCustomerAsync(string id)
        {
            var customer = await _customerCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<CustomerCollection, Customer>(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            var customerCollection = _mapper.Map<Customer, CustomerCollection>(customer);
            await _customerCollection.ReplaceOneAsync(x => x.Id == customer.Id, customerCollection);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            var addressCollection = _mapper.Map<Address, AddressCollection>(address);
            await _addressCollection.ReplaceOneAsync(x => x.Id == address.Id, addressCollection);
        }
    }
}