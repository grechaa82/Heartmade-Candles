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

            var (newCustomer, errors) = Customer.Create(
                name,
                surname,
                middleName,
                phone,
                customer.Address,
                typeDelivery,
                customer.Id);

            if (errors.Any())
            {
                throw new Exception();
            }

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

            var (address, errors) = Address.Create(
                country,
                cities,
                street,
                house,
                flat,
                index,
                customer.Address.Id);

            if (errors.Any())
            {
                throw new Exception();
            }

            await _userRepository.UpdateCustomerAsync(customer);
            await _userRepository.UpdateAddressAsync(address);
        }
    }
}
