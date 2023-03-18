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

        public async Task<User> GetUserAsync(string email, string password)
        {
            var user = await _authRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Password != new SHA().GenerateSHA512(password))
            {
                throw new Exception();
            }
            return user;
        }

        public async Task<bool> RegisterUserAsync(string nickName, string email, string password)
        {
            if (await _authRepository.GetUserByEmailAsync(email) != null)
            {
                return false;
            }

            var address = await _authRepository.CreateAddressAsync();

            var customer = await _authRepository.CreateCustomerAsync(address);

            var (user, errors) = User.Create(
                nickName,
                email,
                password,
                customer.Id,
                default,
                default);

            if (errors.Any())
            {
                throw new Exception();
            }

            await _authRepository.CreateUserAsync(user);

            return true;
        }
    }
}
