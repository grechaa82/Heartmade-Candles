using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.UserAndAuth.DAL;

public static class UserAndAuthDbContextRegistration
{
    public static IServiceCollection AddUserAndAuthDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserAndAuthDbContext>(
            options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
            });

        return services;
    }
}