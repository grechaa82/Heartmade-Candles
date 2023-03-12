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
            return await _userRepository.GetUserAsync(id);
        }

        public async Task UpdateCustomerAsync(
            string userId,
            string name,
            string surname,
            string middleName,
            string phone,
            TypeDelivery typeDelivery)
        {
            var user = await _userRepository.GetUserAsync(userId);

            var customer = await _userRepository.GetCustomerAsync(user.CustomerId);

            var newCustomer = new Customer(
                customer.Id,
                name,
                surname,
                middleName,
                phone,
                customer.Address,
                typeDelivery);

            await _userRepository.UpdateCustomerAsync(newCustomer);
        }

        public async Task UpdateAddressAsync(
            string userId,
            string country,
            string cities,
            string street,
            string house,
            string flat,
            string index)
        {
            var user = await _userRepository.GetUserAsync(userId);

            var customer = await _userRepository.GetCustomerAsync(user.CustomerId);

            var address = new Address(
                customer.Address.Id,
                country,
                cities,
                street,
                house,
                flat,
                index);

            await _userRepository.UpdateCustomerAsync(customer);
            await _userRepository.UpdateAddressAsync(address);
        }
    }
}