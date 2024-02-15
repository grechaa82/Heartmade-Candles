using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Bot.ReplyMarkups;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;

public class GetCompletedOrderHandler : CallBackQueryHandlerBase
{
    public GetCompletedOrderHandler(
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

        return text.Contains(TelegramCallBackQueryCommands.CompletedOrderNextCommand) 
            || text.Contains(TelegramCallBackQueryCommands.CompletedOrderPreviousCommand) 
            || text.Contains(TelegramCallBackQueryCommands.CompletedOrderSelectCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderReplyMarkup.GetBackSelectionMarkup();
        
        if(callbackQuery.Data.ToLower().Contains(TelegramCallBackQueryCommands.CompletedOrderSelectCommand))
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

        var orderMaybe = await GetOrdersByStatus(OrderStatus.Completed, 1, pageIndex - 1);

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
            previousCommands: TelegramCallBackQueryCommands.CompletedOrderPreviousCommand,
            nextCommands: TelegramCallBackQueryCommands.CompletedOrderNextCommand,
            selectCommands: TelegramCallBackQueryCommands.CompletedOrderSelectCommand,
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
