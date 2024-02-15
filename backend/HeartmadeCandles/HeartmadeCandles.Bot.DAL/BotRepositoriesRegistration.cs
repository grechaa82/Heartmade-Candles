using HeartmadeCandles.Bot.Core;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Bot.DAL;

public static class BotRepositoriesRegistration
{
    public static IServiceCollection AddBotRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITelegramBotRepository, TelegramBotRepository>();

        return services;
    }
}