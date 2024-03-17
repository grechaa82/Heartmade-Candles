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

    public async Task<Maybe<User>> GetById(int userId)
    {
        return await _userRepository.GetById(userId);
    }

    public async Task<Maybe<User>> GetByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }

    public async Task<Result> Create(User user)
    {
        var userResult = await _userRepository.GetByEmail(user.Email);

        if (userResult.HasValue)
        {
            return Result.Failure($"{nameof(User)} already exists");
        }

        return await _userRepository.Create(user);
    }

    public async Task<Result> Update(User user)
    {
        return await _userRepository.Update(user);
    }

    public async Task<Result> Delete(int userId)
    {
        return await _userRepository.Delete(userId);
    }
}
