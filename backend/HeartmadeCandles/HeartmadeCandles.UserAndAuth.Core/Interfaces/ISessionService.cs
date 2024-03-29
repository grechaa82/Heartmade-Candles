using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Models;

namespace HeartmadeCandles.UserAndAuth.Core.Interfaces;

public interface ISessionService
{
    Task<Result<Session>> GetById(Guid sessionId);

    Task<Result<Session>> GetByUserId(int userId);

    Task<Result<Session>> Create(Session session);

    Task<Result<Session>> Update(Session newSession);
}
