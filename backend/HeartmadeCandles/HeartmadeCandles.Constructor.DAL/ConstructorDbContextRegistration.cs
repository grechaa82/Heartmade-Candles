using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HeartmadeCandles.Constructor.DAL;

public static class ConstructorDbContextRegistration
{
    public static IServiceCollection AddConstructorDbContext(this IServiceCollection services,
        IConfiguration configuration,
        NpgsqlDataSource builder,
        ILoggerFactory loggerFactory)
    {
        return services.AddDbContext<ConstructorDbContext>(options =>
        {
            options
            .UseNpgsql(builder, options =>
            {
                options.MigrationsAssembly("HeartmadeCandles.Migrations");
            })
            .UseLoggerFactory(loggerFactory);
        });
    }
}