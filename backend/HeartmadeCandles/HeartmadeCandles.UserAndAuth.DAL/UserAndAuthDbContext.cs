using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.UserAndAuth.DAL;

public class UserAndAuthDbContext : DbContext
{
    public UserAndAuthDbContext(DbContextOptions<UserAndAuthDbContext> options) : base(options)
    {
    }
}