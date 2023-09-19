using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Constructor.DAL;

public static class ConstructorRepositoriesRegistration
{
    public static IServiceCollection AddConstructorRepositories(this IServiceCollection services)
    {
        services.AddScoped<IConstructorRepository, ConstructorRepository>();

        return services;
    }
}