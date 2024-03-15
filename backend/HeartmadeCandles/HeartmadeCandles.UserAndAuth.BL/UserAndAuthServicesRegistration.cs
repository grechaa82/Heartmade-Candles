using HeartmadeCandles.UserAndAuth.BL.Services;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.UserAndAuth.BL;

public static class UserAndAuthServicesRegistration
{
    public static IServiceCollection AddUserAndAuthServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IUserService, UserService>();

        return services;
    }
}