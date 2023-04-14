using HeartmadeCandles.Modules.Admin.Core.Models;
using HeartmadeCandles.Modules.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CandleEntity> Candle { get; set; }
    public DbSet<TypeCandleEntity> TypeCandle { get; set; }
}