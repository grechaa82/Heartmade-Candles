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

    public async Task<Maybe<Session>> GetById(Guid sessionId)
    {
        return await _sessionRepository.GetById(sessionId);
    }

    public async Task<Maybe<Session>> GetByUserId(int userId)
    {
        return await _sessionRepository.GetByUserId(userId);
    }

    public async Task<Result<Session>> Create(Session session)
    {
        return await _sessionRepository.Create(session);
    }

    public async Task<Result<Session>> Update(Session newSession)
    {
        return await _sessionRepository.Update(newSession);
    }

    public async Task<Result> Delete(Guid sessionId)
    {
        return await _sessionRepository.Delete(sessionId);
    }
}
