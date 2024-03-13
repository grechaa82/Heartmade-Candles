using HeartmadeCandles.UserAndAuth.BL.Services;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.UserAndAuth.BL;

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