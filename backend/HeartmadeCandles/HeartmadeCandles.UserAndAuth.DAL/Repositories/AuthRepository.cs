using HeartmadeCandles.UserAndAuth.Core.Interfaces;

namespace HeartmadeCandles.UserAndAuth.DAL.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserAndAuthDbContext _context;

    public AuthRepository(UserAndAuthDbContext context)
    {
        _context = context;
    }
}
