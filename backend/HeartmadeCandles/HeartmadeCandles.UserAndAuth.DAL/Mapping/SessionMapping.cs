using HeartmadeCandles.UserAndAuth.Core.Models;
using HeartmadeCandles.UserAndAuth.DAL.Entities;

namespace HeartmadeCandles.UserAndAuth.DAL.Mapping;

internal class SessionMapping
{
    public static Session MapToSession(SessionEntity sessionEntity)
    {
        var session = new Session
        {
            Id = sessionEntity.Id,
            UserId = sessionEntity.UserId,
            User = sessionEntity.User != null 
                ? UserMapping.MapToUser(sessionEntity.User)
                : null,
            RefreshToken = sessionEntity.RefreshToken,
            ExpireAt = sessionEntity.ExpireAt,
        };
    
        return session;
    }

    public static SessionEntity MapToSessionEntity(Session session)
    {
        var sessionEntity = new SessionEntity
        {
            Id = session.Id,
            UserId = session.UserId,
            User = session.User != null
                ? UserMapping.MapToUserEntity(session.User) 
                : null,
            RefreshToken = session.RefreshToken,
            ExpireAt = session.ExpireAt,
        };
    
        return sessionEntity;
    }
}
