using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User> GetUserAsync(string email, string password);
        Task<bool> RegisterUserAsync(string nickName, string email, string password);
    }
}