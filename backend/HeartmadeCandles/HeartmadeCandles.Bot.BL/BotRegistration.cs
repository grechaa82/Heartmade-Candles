﻿using HeartmadeCandles.Bot.BL.Handlers;
using HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;
using HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;
using HeartmadeCandles.Bot.Core;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace HeartmadeCandles.Bot.BL;

public static class BotRegistration
{
    public static IServiceCollection AddBotServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ITelegramBotClient>(new TelegramBotClient(Environment.GetEnvironmentVariable("VAR_TELEGRAM_API_TOKEN") ?? string.Empty))
            .AddScoped<ITelegramBotUpdateHandler, TelegramBotUpdateHandler>()
            .AddScoped<MessageHandlerBase, OrderAnswerHandler>()
            .AddScoped<MessageHandlerBase, OrderPromptHandler>()
            .AddScoped<MessageHandlerBase, GetOrderInfoHandler>()
            .AddScoped<MessageHandlerBase, GetOrderStatusHandler>()
            .AddScoped<MessageHandlerBase, FullNamePromptHandler>()
            .AddScoped<MessageHandlerBase, FullNameAnswerHandler>()
            .AddScoped<MessageHandlerBase, PhoneAnswerHandler>()
            .AddScoped<MessageHandlerBase, AddressAnswerHandler>()
            .AddScoped<MessageHandlerBase, GetOrdersByStatusPromptHandler>()
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
