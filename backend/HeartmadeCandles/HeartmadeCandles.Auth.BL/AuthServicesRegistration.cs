using HeartmadeCandles.Auth.Core;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Auth.BL;

public static class AuthServicesRegistration
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}