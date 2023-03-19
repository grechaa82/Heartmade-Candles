using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetAsync(string id);
        Task<(bool, ErrorDetail[])> UpdateCustomerAsync(string userId, string name, string surname, string middleName, string phone, TypeDelivery typeDelivery);
        Task<(bool, ErrorDetail[])> UpdateAddressAsync(string userId, string country, string cities, string street, string house, string flat, string index);
    }
}
