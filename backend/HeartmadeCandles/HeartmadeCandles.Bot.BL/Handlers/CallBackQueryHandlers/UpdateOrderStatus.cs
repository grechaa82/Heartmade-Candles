using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Bot.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Order.Core.Models;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Bot.Core;

namespace HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;

public class UpdateOrderStatus : CallBackQueryHandlerBase
{   
    public UpdateOrderStatus(
       ITelegramBotClient botClient,
       ITelegramBotRepository telegramBotRepository,
       IServiceScopeFactory serviceScopeFactory)
       : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(CallbackQuery callbackQuery, TelegramUser user)
    {
        if (user.Role != TelegramUserRole.Admin || callbackQuery.Data == null)
        {
            return false;
        }

        var text = callbackQuery.Data.ToLower();

        return text.Contains(TelegramCallBackQueryCommands.UpdateOrderStatusCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToCreatedCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToConfirmedCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToPlacedCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToPaidCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToInProgressCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToPackedCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToInDeliveryCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToCompletedCommand)
            || text.Contains(TelegramCallBackQueryCommands.UpdateToCancelledCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderReplyMarkup.GetBackSelectionMarkup();

        if (callbackQuery.Data.ToLower().Contains(TelegramCallBackQueryCommands.UpdateOrderStatusCommand))
        {
            var selectingInlineKeyboard = OrderReplyMarkup.GetMarkupForSelectingNewOrderStatus(callbackData.Last());

            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: $"Выберите новый статус заказа для заказа {callbackData.Last()}",
                replyMarkup: selectingInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        var command = callbackQuery.Data.ToLower();
        OrderStatus newStatus;

        if (command.Contains(TelegramCallBackQueryCommands.UpdateToCreatedCommand))
        {
            newStatus = OrderStatus.Created;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToConfirmedCommand))
        {
            newStatus = OrderStatus.Confirmed;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToPlacedCommand))
        {
            newStatus = OrderStatus.Placed;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToPaidCommand))
        {
            newStatus = OrderStatus.Paid;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToInProgressCommand))
        {
            newStatus = OrderStatus.InProgress;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToPackedCommand))
        {
            newStatus = OrderStatus.Packed;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToInDeliveryCommand))
        {
            newStatus = OrderStatus.InDelivery;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToCompletedCommand))
        {
            newStatus = OrderStatus.Completed;
        }
        else if (command.Contains(TelegramCallBackQueryCommands.UpdateToCancelledCommand))
        {
            newStatus = OrderStatus.Cancelled;
        }
        else
        {
            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Что-то пошло не так. Не смогли найти нужный статус",
                replyMarkup: backInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        var result = await UpdateOrderStatusInternal(callbackData.Last(), newStatus);

        if (result.IsFailure)
        {
            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: $"Что-то пошло не так, возможная ошибка {result.Error}",
                replyMarkup: backInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        await _botClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: "Новый статус установлен",
            replyMarkup: backInlineKeyboard,
            parseMode: ParseMode.MarkdownV2);

        return;
    }

    private async Task<Result> UpdateOrderStatusInternal(string orderId, OrderStatus orderStatus)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.UpdateOrderStatus(orderId, orderStatus);

        return orderResult;
    }
}