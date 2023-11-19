using HeartmadeCandles.Order.BL.Services;
using HeartmadeCandles.Order.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Order.BL;

public static class OrderServicesRegistration
{
    public static IServiceCollection AddOrderServices(this IServiceCollection services)
    {
        services
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<ICalculateService, CalculateService>()
            .AddScoped<IBasketService, BasketService>();

        return services;
    }
}