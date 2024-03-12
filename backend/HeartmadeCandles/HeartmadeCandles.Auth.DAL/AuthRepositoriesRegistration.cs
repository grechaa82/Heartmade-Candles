using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Admin.DAL;

public static class AuthRepositoriesRegistration
{
    public static IServiceCollection AddAdminRepositories(this IServiceCollection services)
    {
        return services;
    }
}