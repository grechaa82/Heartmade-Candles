using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetAsync(string id)
        {
            return await _userRepository.GetAsync(id);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _userRepository.UpdateCustomerAsync(customer);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            await _userRepository.UpdateAddressAsync(address);
        }
    }
}