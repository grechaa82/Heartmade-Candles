using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HeartmadeCandles.UserAndAuth.DAL;

public static class UserAndAuthDbContextRegistration
{
    public static IServiceCollection AddUserAndAuthDbContext(this IServiceCollection services, 
        IConfiguration configuration,
        NpgsqlDataSource builder,
        ILoggerFactory loggerFactory) 
    {
        return services.AddDbContext<UserAndAuthDbContext>(options =>
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