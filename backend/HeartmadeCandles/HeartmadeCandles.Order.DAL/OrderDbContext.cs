using HeartmadeCandles.Order.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Order.DAL;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<CandleEntity> Candle { get; set; }
    public DbSet<TypeCandleEntity> TypeCandle { get; set; }
    public DbSet<DecorEntity> Decor { get; set; }
    public DbSet<LayerColorEntity> LayerColor { get; set; }
    public DbSet<SmellEntity> Smell { get; set; }
    public DbSet<WickEntity> Wick { get; set; }
    public DbSet<NumberOfLayerEntity> NumberOfLayer { get; set; }
    public DbSet<CandleEntityDecorEntity> CandleDecor { get; set; }
    public DbSet<CandleEntityLayerColorEntity> CandleLayerColor { get; set; }
    public DbSet<CandleEntityNumberOfLayerEntity> CandleNumberOfLayer { get; set; }
    public DbSet<CandleEntitySmellEntity> CandleSmell { get; set; }
    public DbSet<CandleEntityWickEntity> CandleWick { get; set; }
}