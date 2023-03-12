using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetUserAsync(string id);
        Task<Customer> GetCustomerAsync(string id);
        
        Task UpdateCustomerAsync(Customer customer);
        Task UpdateAddressAsync(Address address);
    }
}