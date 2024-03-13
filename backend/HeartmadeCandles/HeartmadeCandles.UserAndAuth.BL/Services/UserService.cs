using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.BL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<Maybe<User>> GetById(int userId)
    {
        return _userRepository.GetById(userId);
    }

    public Task<Maybe<User>> GetByEmail(string email)
    {
        _userRepository.GetByEmail(email);
        throw new NotImplementedException();
    }

    public Task<Result> Create(User user)
    {
        return _userRepository.Create(user);
    }
}
