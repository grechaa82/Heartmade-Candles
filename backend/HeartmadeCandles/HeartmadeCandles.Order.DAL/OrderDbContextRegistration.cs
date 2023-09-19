using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Order.DAL;

public static class OrderDbContextRegistration
{
    public static IServiceCollection AddOrderDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(
            options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
            });

        return services;
    }
}