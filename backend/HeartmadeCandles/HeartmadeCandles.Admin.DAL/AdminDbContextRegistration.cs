using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HeartmadeCandles.Admin.DAL;

public static class AdminDbContextRegistration
{
    public static IServiceCollection AddAdminDbContext(this IServiceCollection services, 
        IConfiguration configuration,
        NpgsqlDataSource builder,
        ILoggerFactory loggerFactory)
    {
        return services.AddDbContext<AdminDbContext>(options =>
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