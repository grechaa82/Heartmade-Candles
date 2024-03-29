using HeartmadeCandles.UserAndAuth.Core.Interfaces;
using HeartmadeCandles.UserAndAuth.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.UserAndAuth.DAL;

public static class UserAndAuthRepositoriesRegistration
{
    public static IServiceCollection AddUserAndAuthRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<ISessionRepository, SessionRepository>()
            .AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}