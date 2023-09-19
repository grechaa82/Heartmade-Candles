using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Constructor.DAL;

public static class ConstructorDbContextRegistration
{
    public static IServiceCollection AddConstructorDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ConstructorDbContext>(
            options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
            });

        return services;
    }
}