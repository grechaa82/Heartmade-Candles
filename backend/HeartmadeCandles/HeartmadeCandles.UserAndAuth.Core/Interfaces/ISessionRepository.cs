using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.Core.Interfaces;

public interface ISessionRepository
{
    Task<Maybe<Session>> GetById(Guid sessionId);

    Task<Maybe<Session>> GetByUserId(int userId);

    Task<Result<Session>> Create(Session session);

    Task<Result<Session>> Update(Session newSession);
}
