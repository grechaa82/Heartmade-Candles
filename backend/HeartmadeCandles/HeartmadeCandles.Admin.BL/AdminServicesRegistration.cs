using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HeartmadeCandles.Admin.BL;

public static class AdminServicesRegistration
{
    public static IServiceCollection AddAdminServices(this IServiceCollection services, string pathToStaticFilesImages)
    {
        services
            .AddScoped<ICandleService, CandleService>()
            .AddScoped<IDecorService, DecorService>()
            .AddScoped<ILayerColorService, LayerColorService>()
            .AddScoped<ISmellService, SmellService>()
            .AddScoped<IWickService, WickService>()
            .AddScoped<INumberOfLayerService, NumberOfLayerService>()
            .AddScoped<ITypeCandleService, TypeCandleService>()
            .AddScoped<IImageService>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ImageService>>();
                return new ImageService(pathToStaticFilesImages, logger);
            });

        return services;
    }
}