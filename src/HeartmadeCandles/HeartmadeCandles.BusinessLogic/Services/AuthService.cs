using HeartmadeCandles.Core;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<bool> IsVerifyAsync(string email, string password)
        {
            var user = await _authRepository.GetUserByEmailAsync(email);

            if (user != null)
            {
                if (user.Password == new SHA().GenerateSHA512(password))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> RegisterUserAsync(string nickName, string email, string password)
        {
            if (await _authRepository.GetUserByEmailAsync(email) != null)
            {
                return false;
            }
            var user = new User(nickName,
                email,
                new SHA().GenerateSHA512(password),
                default,
                default);

            await _authRepository.CreateUserAsync(user);

            return true;
        }
    }
}