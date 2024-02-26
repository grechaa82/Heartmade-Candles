using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Order.Core.Models;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;

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

        return text.Contains(CallBackQueryCommands.UpdateOrderStatusCommand)
            || text.Contains(CallBackQueryCommands.UpdateToCreatedCommand)
            || text.Contains(CallBackQueryCommands.UpdateToConfirmedCommand)
            || text.Contains(CallBackQueryCommands.UpdateToPlacedCommand)
            || text.Contains(CallBackQueryCommands.UpdateToPaidCommand)
            || text.Contains(CallBackQueryCommands.UpdateToInProgressCommand)
            || text.Contains(CallBackQueryCommands.UpdateToPackedCommand)
            || text.Contains(CallBackQueryCommands.UpdateToInDeliveryCommand)
            || text.Contains(CallBackQueryCommands.UpdateToCompletedCommand)
            || text.Contains(CallBackQueryCommands.UpdateToCancelledCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderInlineKeyboardMarkup.GetBackSelectionMarkup();

        if (callbackQuery.Data.ToLower().Contains(CallBackQueryCommands.UpdateOrderStatusCommand))
        {
            var selectingInlineKeyboard = OrderInlineKeyboardMarkup.GetMarkupForSelectingNewOrderStatus(callbackData.Last());

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

        if (command.Contains(CallBackQueryCommands.UpdateToCreatedCommand))
        {
            newStatus = OrderStatus.Created;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToConfirmedCommand))
        {
            newStatus = OrderStatus.Confirmed;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToPlacedCommand))
        {
            newStatus = OrderStatus.Placed;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToPaidCommand))
        {
            newStatus = OrderStatus.Paid;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToInProgressCommand))
        {
            newStatus = OrderStatus.InProgress;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToPackedCommand))
        {
            newStatus = OrderStatus.Packed;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToInDeliveryCommand))
        {
            newStatus = OrderStatus.InDelivery;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToCompletedCommand))
        {
            newStatus = OrderStatus.Completed;
        }
        else if (command.Contains(CallBackQueryCommands.UpdateToCancelledCommand))
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