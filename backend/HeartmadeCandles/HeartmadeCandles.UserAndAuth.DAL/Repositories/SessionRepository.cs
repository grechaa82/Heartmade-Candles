using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;
using HeartmadeCandles.UserAndAuth.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.UserAndAuth.DAL.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly UserAndAuthDbContext _context;

    public SessionRepository(UserAndAuthDbContext context)
    {
        _context = context;
    }

    public Task<Maybe<Session>> GetById(Guid sessionId)
    {
        throw new NotImplementedException();
    }

    public async Task<Maybe<Session>> GetByUserId(int userId)
    {
        var item = await _context.Session
            .AsNoTracking()
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (item == null)
        {
            return Maybe<Session>.None;
        }

        var result = SessionMapping.MapToSession(item);

        return result;
    }

    public async Task<Result<Session>> Create(Session session)
    {
        var item = SessionMapping.MapToSessionEntity(session);

        await _context.Session.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success(session)
            : Result.Failure<Session>("Session was not created");
    }

    public async Task<Result<Session>> Update(Session newSession)
    {
        var item = SessionMapping.MapToSessionEntity(newSession);

        _context.Session.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success(newSession)
            : Result.Failure<Session>("Session was not updated");
    }
}
