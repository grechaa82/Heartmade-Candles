using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Bot.ReplyMarkups;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.Handlers.CallBackQueryHandlers;

public class GetPackedOrderHandler : CallBackQueryHandlerBase
{
    public GetPackedOrderHandler(
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

        return text.Contains(TelegramCallBackQueryCommands.PackedOrderNextCommand) 
            || text.Contains(TelegramCallBackQueryCommands.PackedOrderPreviousCommand) 
            || text.Contains(TelegramCallBackQueryCommands.PackedOrderSelectCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderReplyMarkup.GetBackSelectionMarkup();
        
        if(callbackQuery.Data.ToLower().Contains(TelegramCallBackQueryCommands.PackedOrderSelectCommand))
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

            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: OrderInfoFormatter.GetOrderInfoInMarkdownV2(orderResult.Value),
                replyMarkup: backInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        var pageIndex = int.Parse(callbackData.Last());

        var orderMaybe = await GetOrdersByStatus(OrderStatus.Packed, 1, pageIndex - 1);

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
            previousCommands: TelegramCallBackQueryCommands.PackedOrderPreviousCommand,
            nextCommands: TelegramCallBackQueryCommands.PackedOrderNextCommand,
            selectCommands: TelegramCallBackQueryCommands.PackedOrderSelectCommand,
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
