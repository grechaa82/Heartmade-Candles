using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmail(string email);
    }
}