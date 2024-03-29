using HeartmadeCandles.UserAndAuth.BL.Services;
using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.UserAndAuth.BL;

public static class UserAndAuthServicesRegistration
{
    public static IServiceCollection AddUserAndAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<ISessionService, SessionService>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IUserService, UserService>()
            .Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        return services;
    }
}