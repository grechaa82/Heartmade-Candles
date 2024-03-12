using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
}