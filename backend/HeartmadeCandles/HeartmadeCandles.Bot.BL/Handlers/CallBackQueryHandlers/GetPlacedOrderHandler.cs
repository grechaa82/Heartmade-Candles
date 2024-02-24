using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Bot.ReplyMarkups;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;

public class GetPaidOrderHandler : CallBackQueryHandlerBase
{
    public GetPaidOrderHandler(
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

        return text.Contains(TelegramCallBackQueryCommands.PaidOrderNextCommand) 
            || text.Contains(TelegramCallBackQueryCommands.PaidOrderPreviousCommand) 
            || text.Contains(TelegramCallBackQueryCommands.PaidOrderSelectCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderInlineKeyboardMarkup.GetBackSelectionMarkup();
        
        if(callbackQuery.Data.ToLower().Contains(TelegramCallBackQueryCommands.PaidOrderSelectCommand))
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

            var selectedOrderInlineKeyboard = OrderInlineKeyboardMarkup.GetMarkupOfSelectedOrder(orderResult.Value.Id);

            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: OrderInfoFormatter.GetOrderInfoInMarkdownV2(orderResult.Value),
                replyMarkup: selectedOrderInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        var pageIndex = int.Parse(callbackData.Last());

        var (orderMaybe, totalOrders) = await GetOrderByStatusWithTotalOrders(OrderStatus.Paid, 1, pageIndex - 1);

        if (orderMaybe.HasNoValue || orderMaybe.Value.First() == null)
        {
            await _botClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Ничего не найдено",
                replyMarkup: backInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);
        }

        var inlineKeyboard = OrderInlineKeyboardMarkup.GetOrderSelectionMarkup(
            previousCommands: TelegramCallBackQueryCommands.PaidOrderPreviousCommand,
            nextCommands: TelegramCallBackQueryCommands.PaidOrderNextCommand,
            selectCommands: TelegramCallBackQueryCommands.PaidOrderSelectCommand,
            orderId: orderMaybe.Value.First().Id,
            currentPageIndex: pageIndex,
            totalOrders: totalOrders);

        await _botClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: OrderInfoFormatter.GetPreviewOrderInfoInMarkdownV2(orderMaybe.Value.First()),
            replyMarkup: inlineKeyboard,
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
