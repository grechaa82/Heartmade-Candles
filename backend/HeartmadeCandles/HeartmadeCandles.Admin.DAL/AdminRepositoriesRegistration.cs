using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Admin.DAL;

public static class AdminRepositoriesRegistration
{
    public static IServiceCollection AddAdminRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<ICandleRepository, CandleRepository>()
            .AddScoped<IDecorRepository, DecorRepository>()
            .AddScoped<ILayerColorRepository, LayerColorRepository>()
            .AddScoped<ISmellRepository, SmellRepository>()
            .AddScoped<IWickRepository, WickRepository>()
            .AddScoped<INumberOfLayerRepository, NumberOfLayerRepository>()
            .AddScoped<ITypeCandleRepository, TypeCandleRepository>();

        return services;
    }
}