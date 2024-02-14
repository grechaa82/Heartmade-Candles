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
            .AddTransient<MessageHandlerBase, OrderAnswerHandler>()
            .AddTransient<MessageHandlerBase, OrderPromptHandler>()
            .AddTransient<MessageHandlerBase, GetOrderInfoHandler>()
            .AddTransient<MessageHandlerBase, GetOrderStatusHandler>()
            .AddTransient<MessageHandlerBase, FullNamePromptHandler>()
            .AddTransient<MessageHandlerBase, FullNameAnswerHandler>()
            .AddTransient<MessageHandlerBase, PhoneAnswerHandler>()
            .AddTransient<MessageHandlerBase, AddressAnswerHandler>()
            .AddTransient<MessageHandlerBase, GetOrdersByStatusPromptHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetOrdersHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetCreatedOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetConfirmedOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetPlacedOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetPaidOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetInProgressOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetPackedOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetInDeliveryOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetCompletedOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetCancelledOrderHandler>()
            .AddTransient<CallBackQueryHandlerBase, GetOrderIdHandler>()
            .AddSingleton(provider =>
            {
                return new TelegramBotUpdateHandler(
                    provider.GetRequiredService<ITelegramBotClient>(),
                    provider.GetRequiredService<IServiceScopeFactory>(),
                    provider.GetRequiredService<IMongoDatabase>(),
                    provider.GetServices<MessageHandlerBase>(),
                    provider.GetServices<CallBackQueryHandlerBase>(),
                    provider.GetRequiredService<ILogger<TelegramBotUpdateHandler>>()
                );
            });

        return services;
    }
}
