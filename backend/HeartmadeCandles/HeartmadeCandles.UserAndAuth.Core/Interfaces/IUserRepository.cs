using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.Core.Interfaces;

public interface IUserRepository
{
    Task<Maybe<User>> GetById(int userId);

    Task<Maybe<User>> GetByEmail(string email);

    Task<Result> Create(User user);
}
