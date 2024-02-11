using HeartmadeCandles.Bot.Handlers;
using HeartmadeCandles.Bot.Handlers.CallBackQueryHandlers;
using HeartmadeCandles.Bot.Handlers.MessageHandlers;
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
            .AddSingleton<ITelegramBotUpdateHandler, TelegramBotUpdateHandler>()
            .AddTransient<HandlerMessageBase, OrderAnswerHandler>()
            .AddTransient<HandlerMessageBase, OrderPromptHandler>()
            .AddTransient<HandlerMessageBase, GetOrderInfoHandler>()
            .AddTransient<HandlerMessageBase, GetOrderStatusHandler>()
            .AddTransient<HandlerMessageBase, FullNamePromptHandler>()
            .AddTransient<HandlerMessageBase, FullNameAnswerHandler>()
            .AddTransient<HandlerMessageBase, PhoneAnswerHandler>()
            .AddTransient<HandlerMessageBase, AddressAnswerHandler>()
            .AddTransient<HandlerMessageBase, GetOrdersByStatusPromptHandler>()
            .AddTransient<HandlerCallBackQueryBase, GetCreatedOrderHandler>()
            .AddSingleton(provider =>
            {
                return new TelegramBotUpdateHandler(
                    provider.GetRequiredService<ITelegramBotClient>(),
                    provider.GetRequiredService<IServiceScopeFactory>(),
                    provider.GetRequiredService<IMongoDatabase>(),
                    provider.GetServices<HandlerMessageBase>(),
                    provider.GetServices<HandlerCallBackQueryBase>(),
                    provider.GetRequiredService<ILogger<TelegramBotUpdateHandler>>()
                );
            });

        return services;
    }
}
