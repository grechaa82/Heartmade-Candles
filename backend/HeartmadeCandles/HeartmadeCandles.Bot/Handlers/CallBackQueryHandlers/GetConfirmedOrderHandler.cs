using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Bot.ReplyMarkups;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.Handlers.CallBackQueryHandlers;

public class GetConfirmedOrderHandler : CallBackQueryHandlerBase
{
    public GetConfirmedOrderHandler(
       ITelegramBotClient botClient,
       IMongoDatabase mongoDatabase,
       IServiceScopeFactory serviceScopeFactory)
       : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(CallbackQuery callbackQuery, TelegramUser user)
    {
        if (user.Role != TelegramUserRole.Admin || callbackQuery.Data == null)
        {
            return false;
        }

        var text = callbackQuery.Data.ToLower();

        return text.Contains(TelegramCallBackQueryCommands.ConfirmedOrderPreviousCommand)
            || text.Contains(TelegramCallBackQueryCommands.ConfirmedOrderNextCommand)
            || text.Contains(TelegramCallBackQueryCommands.ConfirmedOrderSelectCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderReplyMarkup.GetBackSelectionMarkup();
        
        if (callbackQuery.Data.ToLower().Contains(TelegramCallBackQueryCommands.ConfirmedOrderSelectCommand))
        {
            var orderResult = await GetOrderById(callbackData.Last());

            if (orderResult.IsFailure)
            {
                await _botClient.EditMessageTextAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    messageId: callbackQuery.Message.MessageId,
                    text: "Что-то пошло не так",
                    replyMarkup: backInlineKeyboard,
                    parseMode: ParseMode.MarkdownV2);
            }

            var selectedOrderInlineKeyboard = OrderReplyMarkup.GetMarkupOfSelectedOrder(orderResult.Value.Id);

            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: OrderInfoFormatter.GetOrderInfoInMarkdownV2(orderResult.Value),
                replyMarkup: selectedOrderInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        var pageIndex = int.Parse(callbackData.Last());

        var orderMaybe = await GetOrdersByStatus(OrderStatus.Confirmed, 1, pageIndex - 1);

        if (orderMaybe.HasNoValue)
        {
            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Ничего не найдено",
                replyMarkup: backInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);
        }

        var inlineKeyboard = OrderReplyMarkup.GetOrderSelectionMarkup(
            previousCommands: TelegramCallBackQueryCommands.ConfirmedOrderPreviousCommand,
            nextCommands: TelegramCallBackQueryCommands.ConfirmedOrderNextCommand,
            selectCommands: TelegramCallBackQueryCommands.ConfirmedOrderSelectCommand,
            orderId: orderMaybe.Value.Id,
            currentPageIndex: pageIndex);

        await _botClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: OrderInfoFormatter.GetPreviewOrderInfoInMarkdownV2(orderMaybe.Value),
            replyMarkup: inlineKeyboard,
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
