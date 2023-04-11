using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task<Address> CreateAddressAsync();
        Task<Customer> CreateCustomerAsync(Address address);
    }
}
