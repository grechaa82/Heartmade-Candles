using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;
using System.Net.NetworkInformation;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMongoCollection<UserCollection> _userCollection;
        private readonly IMongoCollection<AddressCollection> _addressCollection;
        private readonly IMongoCollection<CustomerCollection> _customerCollection;
        private readonly IMapper _mapper;

        public AuthRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _userCollection = mongoDatabase.GetCollection<UserCollection>("User");
            _addressCollection = mongoDatabase.GetCollection<AddressCollection>("Address");
            _customerCollection = mongoDatabase.GetCollection<CustomerCollection>("Customer");
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

        public async Task<Address> CreateAddressAsync()
        {
            var newAddress = new AddressCollection();
            await _addressCollection.InsertOneAsync(newAddress);
            return _mapper.Map<AddressCollection, Address>(newAddress);
        }

        public async Task<Customer> CreateCustomerAsync(Address address)
        {
            var newCustomer = new CustomerCollection();
            newCustomer.Address = address;
            await _customerCollection.InsertOneAsync(newCustomer);
            return _mapper.Map<CustomerCollection, Customer>(newCustomer);
        }
    }
}
