using HeartmadeCandles.Order.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Order.Bot;

public static class OrderNotificationServicesRegistration
{
    public static IServiceCollection AddOrderNotificationServices(this IServiceCollection services)
    {
        services
            .AddScoped<IOrderNotificationHandler, OrderNotificationHandler>()
            .AddSingleton<ITelegramUserCache, TelegramUserCache>();

        return services;
    }
}