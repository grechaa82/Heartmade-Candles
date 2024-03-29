using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.BL.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Result<Session>> GetById(Guid sessionId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Session>> GetByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Session>> Create(Session session)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Session>> Update(Session newSession)
    {
        throw new NotImplementedException();
    }
}
