using HeartmadeCandles.Constructor.BL.Services;
using HeartmadeCandles.Constructor.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Constructor.BL;

public static class ConstructorServicesRegistration
{
    public static IServiceCollection AddConstructorServices(this IServiceCollection services)
    {
        services.AddScoped<IConstructorService, ConstructorService>();

        return services;
    }
}