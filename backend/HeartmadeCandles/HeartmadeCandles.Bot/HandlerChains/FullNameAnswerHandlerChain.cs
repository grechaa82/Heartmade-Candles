﻿using HeartmadeCandles.Bot.Documents;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.HandlerChains;

public class FullNameAnswerHandlerChain : HandlerChainBase
{
    public FullNameAnswerHandlerChain(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingFullName;

    public async override Task Process(Message message, TelegramUser user)
    {
        var update = Builders<TelegramUser>.Update
            .Set(x => x.State, TelegramUserState.AskingPhone);

        await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == user.ChatId, update: update);

        await ForwardFullNameToAdminAsync(_botClient, message, user);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                """
                Шаг 2 из 3
                
                Введите ваш номер телефона.
                
                Пример: +7 987 654 32 10
                """),
            parseMode: ParseMode.MarkdownV2);

        return;
    }

    private async Task ForwardFullNameToAdminAsync(
        ITelegramBotClient botClient, 
        Message message, 
        TelegramUser user, 
        CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом {user.CurrentOrderId} заполнил свое ФИО:"),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        await botClient.ForwardMessageAsync(
            chatId: _adminChatId,
            fromChatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken);
    }

}