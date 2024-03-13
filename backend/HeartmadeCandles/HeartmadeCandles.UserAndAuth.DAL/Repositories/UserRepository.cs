using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserAndAuthDbContext _context;

    public UserRepository(UserAndAuthDbContext context)
    {
        _context = context;
    }

    public Task<Maybe<User>> GetById(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<User>> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }
    
    public Task<Result> Create(User user)
    {
        throw new NotImplementedException();
    }
}
