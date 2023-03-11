using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetAsync(string id);
        Task UpdateCustomerAsync(Customer customer);
        Task UpdateAddressAsync(Address address);
    }
}