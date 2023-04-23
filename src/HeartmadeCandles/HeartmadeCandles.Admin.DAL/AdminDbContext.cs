using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

        public DbSet<CandleEntity> Candle { get; set; }
        public DbSet<TypeCandleEntity> TypeCandle { get; set; }
        public DbSet<DecorEntity> Decor { get; set; }
        public DbSet<LayerColorEntity> LayerColor { get; set; }
        public DbSet<SmellEntity> Smell { get; set; }
        public DbSet<WickEntity> Wick { get; set; }
        public DbSet<NumberOfLayerEntity> NumberOfLayer { get; set; }
    }
}
