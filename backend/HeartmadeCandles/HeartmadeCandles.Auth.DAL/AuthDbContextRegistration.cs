using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Admin.DAL;

public static class AuthDbContextRegistration
{
    public static IServiceCollection AddAdminDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}