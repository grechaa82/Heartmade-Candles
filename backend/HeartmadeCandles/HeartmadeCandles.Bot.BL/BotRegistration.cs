using HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;
using HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;
using HeartmadeCandles.Bot.BL.Services;
using HeartmadeCandles.Bot.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace HeartmadeCandles.Bot.BL;

public static class BotRegistration
{
    public static IServiceCollection AddBotServices(this IServiceCollection services, IConfiguration configuration)
    {
        var apiToken = configuration.GetSection("TelegramSettings")["Token"] ?? "";

        services
            .AddScoped<ITelegramBotService, TelegramBotService>()
            .AddSingleton<ITelegramBotClient>(new TelegramBotClient(apiToken))
            .AddScoped<ITelegramBotUpdateHandler, TelegramBotUpdateHandler>()
            .AddScoped<MessageHandlerBase, OrderIdAnswerHandler>()
            .AddScoped<MessageHandlerBase, OrderIdPromptHandler>()
            .AddScoped<MessageHandlerBase, GetOrderInfoHandler>()
            .AddScoped<MessageHandlerBase, GetOrderStatusHandler>()
            .AddScoped<MessageHandlerBase, FullNamePromptHandler>()
            .AddScoped<MessageHandlerBase, FullNameAnswerHandler>()
            .AddScoped<MessageHandlerBase, PhoneAnswerHandler>()
            .AddScoped<MessageHandlerBase, DeliveryTypeAnswerHandler>()
            .AddScoped<MessageHandlerBase, AddressAnswerHandler>()
            .AddScoped<MessageHandlerBase, GetOrdersByStatusPromptHandler>()
            .AddScoped<MessageHandlerBase, GetAdminCommandHandler>()
            .AddScoped<MessageHandlerBase, GetChatIdHandler>()
            .AddScoped<MessageHandlerBase, GetOrderByIdPromptHandler>()
            .AddScoped<MessageHandlerBase, GetOrderByIdAnswerHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetOrdersHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetCreatedOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetConfirmedOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetPlacedOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetPaidOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetInProgressOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetPackedOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetInDeliveryOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetCompletedOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetCancelledOrderHandler>()
            .AddScoped<CallBackQueryHandlerBase, GetOrderIdHandler>()
            .AddScoped<CallBackQueryHandlerBase, UpdateOrderStatus>();

        return services;
    }
}
