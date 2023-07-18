using HeartmadeCandles.Constructor.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Constructor.DAL
{
    public class ConstructorDbContext : DbContext
    {
        public ConstructorDbContext(DbContextOptions<ConstructorDbContext> options) : base(options) { }

        public DbSet<CandleEntity> Candle { get; set; }
    }
}
