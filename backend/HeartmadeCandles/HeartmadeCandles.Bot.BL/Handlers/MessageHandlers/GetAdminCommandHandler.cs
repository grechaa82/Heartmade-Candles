﻿using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.BL.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class GetAdminCommandHandler : MessageHandlerBase
{
    public GetAdminCommandHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user)
    {
        if (user.Role != TelegramUserRole.Admin || message.Text == null)
        {
            return false;
        }

        return message.Text?.ToLower().Contains(TelegramMessageCommands.StartAdminCommand) ?? false;
    }   

    public async override Task Process(Message message, TelegramUser user)
    {
        var replyKeyboardMarkup = AdminReplyKeyboardMarkup.GetAdminCommands();

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Вам доступны команды: 
                    
                {TelegramMessageCommands.GetOrdersByStatusCommand} - работа с заказами
                {TelegramMessageCommands.GetOrderByIdCommand} - получить информацию о заказе по номеру (id)
                """),
            messageThreadId: message.MessageThreadId,
            replyMarkup: replyKeyboardMarkup,
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
