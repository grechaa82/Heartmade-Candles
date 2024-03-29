using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.DAL.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly UserAndAuthDbContext _context;

    public SessionRepository(UserAndAuthDbContext context)
    {
        _context = context;
    }

    public Task<Result<Session>> GetById(Guid sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Session>> GetByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Session>> Create(Session session)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Session>> Update(Session newSession)
    {
        throw new NotImplementedException();
    }
}
