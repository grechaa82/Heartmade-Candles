using CSharpFunctionalExtensions;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.Core.Models;
using HeartmadeCandles.UserAndAuth.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.UserAndAuth.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserAndAuthDbContext _context;

    public UserRepository(UserAndAuthDbContext context)
    {
        _context = context;
    }

    public async Task<Maybe<User>> GetById(int userId)
    {
        var item = await _context.User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (item == null)
        {
            return Maybe<User>.None;
        }

        var result = UserMapping.MapToUser(item);

        return result;
    }

    public async Task<Maybe<User>> GetByEmail(string email)
    {
        var item = await _context.User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (item == null)
        {
            return Maybe<User>.None;
        }

        var result = UserMapping.MapToUser(item);

        return result;
    }
    
    public async Task<Result> Create(User user)
    {
        var item = UserMapping.MapToUserEntity(user);

        await _context.User.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure("User was not created");
    }

    public async Task<Result> Update(User user)
    {
        var item = UserMapping.MapToUserEntity(user);

        _context.User.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure("User was not updated");
    }

    public async Task<Result> Delete(int userId)
    {
        var item = await _context.User.FirstOrDefaultAsync(c => c.Id == userId);

        if (item == null)
        {
            return Result.Failure($"User by id: {userId} does not exist");
        }

        _context.User.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"User by id: {userId} was not deleted");
    }
}
