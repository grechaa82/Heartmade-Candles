using HeartmadeCandles.Core;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
namespace HeartmadeCandles.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<bool> IsVerify(string email, string password)
        {
            var user = await _authRepository.GetUserByEmail(email);

            if (user != null)
            {
                if (user.Password == new SHA().GenerateSHA512(password))
                {
                    return true;
                }
            }
            return false;
        }
    }
}