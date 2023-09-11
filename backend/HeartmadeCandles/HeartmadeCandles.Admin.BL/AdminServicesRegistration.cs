using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Admin.BL
{
    public static class AdminServicesRegistration
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            services
                .AddScoped<ICandleService, CandleService>()
                .AddScoped<IDecorService, DecorService>()
                .AddScoped<ILayerColorService, LayerColorService>()
                .AddScoped<ISmellService, SmellService>()
                .AddScoped<IWickService, WickService>()
                .AddScoped<INumberOfLayerService, NumberOfLayerService>()
                .AddScoped<ITypeCandleService, TypeCandleService>();

            return services;
        }
    }
}
