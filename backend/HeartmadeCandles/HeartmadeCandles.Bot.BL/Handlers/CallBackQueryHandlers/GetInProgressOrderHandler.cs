using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.BL.Utilities;
using HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;

public class GetInProgressOrderHandler : CallBackQueryHandlerBase
{
    public GetInProgressOrderHandler(
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

        return text.Contains(CallBackQueryCommands.InProgressOrderNextCommand) 
            || text.Contains(CallBackQueryCommands.InProgressOrderPreviousCommand) 
            || text.Contains(CallBackQueryCommands.InProgressOrderSelectCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderInlineKeyboardMarkup.GetBackSelectionMarkup();
        
        if(callbackQuery.Data.ToLower().Contains(CallBackQueryCommands.InProgressOrderSelectCommand))
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
                text: OrderReportGenerator.GenerateReport(orderResult.Value),
                replyMarkup: selectedOrderInlineKeyboard,
                parseMode: ParseMode.MarkdownV2);

            return;
        }

        var pageIndex = int.Parse(callbackData.Last());

        var (orderMaybe, totalOrders) = await GetOrderByStatusWithTotalOrders(OrderStatus.InProgress, 1, pageIndex - 1);

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
            previousCommands: CallBackQueryCommands.InProgressOrderPreviousCommand,
            nextCommands: CallBackQueryCommands.InProgressOrderNextCommand,
            selectCommands: CallBackQueryCommands.InProgressOrderSelectCommand,
            orderId: orderMaybe.Value.First().Id,
            currentPageIndex: pageIndex,
            totalOrders: totalOrders);

        await _botClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: OrderReportGenerator.GeneratePreviewReport(orderMaybe.Value.First()),
            replyMarkup: inlineKeyboard,
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
