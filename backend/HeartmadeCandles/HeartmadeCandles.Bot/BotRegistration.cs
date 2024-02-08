using HeartmadeCandles.Bot.HandlerChains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Telegram.Bot;

namespace HeartmadeCandles.Bot;

public static class BotRegistration
{
    public static IServiceCollection AddBot(this IServiceCollection services)
    {
        services
            .AddSingleton<ITelegramBotClient>(new TelegramBotClient(Environment.GetEnvironmentVariable("VAR_TELEGRAM_API_TOKEN") ?? string.Empty))
            .AddSingleton<ITelegramBotService, TelegramBotService>()
            .AddTransient<HandlerChainBase, OrderAnswerHandlerChain>()
            .AddTransient<HandlerChainBase, OrderPromptHandlerChain>()
            .AddTransient<HandlerChainBase, GetOrderInfoHandlerChain>()
            .AddTransient<HandlerChainBase, GetOrderStatusHandlerChain>()
            .AddTransient<HandlerChainBase, FullNamePromptHandlerChain>()
            .AddTransient<HandlerChainBase, FullNameAnswerHandlerChain>()
            .AddTransient<HandlerChainBase, PhoneAnswerHandlerChain>()
            .AddTransient<HandlerChainBase, AddressAnswerHandlerChain>()
            .AddSingleton(provider =>
            {
                return new TelegramBotService(
                    provider.GetRequiredService<ITelegramBotClient>(),
                    provider.GetRequiredService<IServiceScopeFactory>(),
                    provider.GetRequiredService<IMongoDatabase>(),
                    provider.GetServices<HandlerChainBase>(),
                    provider.GetRequiredService<ILogger<TelegramBotService>>()
                );
            });

        return services;
    }
}
