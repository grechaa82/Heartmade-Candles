using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.Core.Interfaces;

public interface IUserService
{
    Task<Maybe<User>> GetById(int userId);

    Task<Maybe<User>> GetByEmail(string email);

    Task<Result> Create(User user);

    Task<Result> Update(User user);

    Task<Result> Delete(int userId);
}
