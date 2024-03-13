using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.UserAndAuth.DAL;

public static class UserAndAuthDbContextRegistration
{
    public static IServiceCollection AddUserAndAuthDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}