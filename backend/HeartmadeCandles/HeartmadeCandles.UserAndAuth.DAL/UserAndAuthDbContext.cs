using HeartmadeCandles.UserAndAuth.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.UserAndAuth.DAL;

public class UserAndAuthDbContext : DbContext
{
    public UserAndAuthDbContext(DbContextOptions<UserAndAuthDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> User { get; set; }
    public DbSet<TokenEntity> Token { get; set; }
}