﻿using HeartmadeCandles.Bot.BL.Utilities;
using HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;
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

        return text.Contains(CallBackQueryCommands.CompletedOrderNextCommand) 
            || text.Contains(CallBackQueryCommands.CompletedOrderPreviousCommand) 
            || text.Contains(CallBackQueryCommands.CompletedOrderSelectCommand);
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        var callbackData = callbackQuery.Data!.Split(":");

        var backInlineKeyboard = OrderInlineKeyboardMarkup.GetBackSelectionMarkup();
        
        if(callbackQuery.Data.ToLower().Contains(CallBackQueryCommands.CompletedOrderSelectCommand))
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

        var (orderMaybe, totalOrders) = await GetOrderByStatusWithTotalOrders(OrderStatus.Completed, 1, pageIndex - 1);

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
            previousCommands: CallBackQueryCommands.CompletedOrderPreviousCommand,
            nextCommands: CallBackQueryCommands.CompletedOrderNextCommand,
            selectCommands: CallBackQueryCommands.CompletedOrderSelectCommand,
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
