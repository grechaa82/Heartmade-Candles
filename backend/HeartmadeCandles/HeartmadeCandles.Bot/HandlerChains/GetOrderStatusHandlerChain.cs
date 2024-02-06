﻿using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Order.Core.Interfaces;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.HandlerChains;

public class GetOrderStatusHandlerChain : HandlerChainBase
{
    public GetOrderStatusHandlerChain(
        ITelegramBotClient botClient,
        ITelegramUserCache userCache,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, userCache, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(TelegramCommands.GetOrderStatusCommand) ?? false;

    public async override Task Process(Message message, TelegramUser user)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(user.CurrentOrderId ?? string.Empty);

        if (orderResult.IsFailure)
        {
            await SendOrderProcessingErrorMessage(_botClient, message.Chat.Id);

            return;
        }

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: orderResult.Value.Status.ToString(),
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task SendOrderProcessingErrorMessage(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Возникла проблема с вашим заказом. Мы не смогли его найти. 
                
                Вы можете:
                - Попробовать ввести номер заказа еще раз {TelegramCommands.InputOrderIdCommand}
                - Создать новый заказ на нашем сайте 4fass.ru
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }
}
