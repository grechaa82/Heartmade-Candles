using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Documents;
using MongoDB.Driver;
using HeartmadeCandles.Bot.Handlers;

namespace HeartmadeCandles.Bot.Handlers.MessageHandlers;

public class AddressAnswerHandler : MessageHandlerBase
{
    public AddressAnswerHandler(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingAddress;

    public async override Task Process(Message message, TelegramUser user)
    {
        var update = Builders<TelegramUser>.Update
            .Set(x => x.State, TelegramUserState.OrderExist);

        await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == user.ChatId, update: update);

        await ForwardAddressToAdminAsync(_botClient, message, user);

        await SendConfirmedAsync(_botClient, message.Chat.Id);

        return;
    }

    private async Task ForwardAddressToAdminAsync(
        ITelegramBotClient botClient,
        Message message,
        TelegramUser user,
        CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом " +
            $"{user.CurrentOrderId} заполнил свой адрес доставки:"),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        await botClient.ForwardMessageAsync(
            chatId: _adminChatId,
            fromChatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken);
    }

    private async Task SendConfirmedAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        // TODO: Изменить статус заказа на оформленный
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Ваши данные успешно напралвлены администратору.
                Вы Можете отслеживать статус заказа используя команду {TelegramMessageCommands.GetOrderStatusCommand}
                
                Если возникнут сложности он с вами свяжется.
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }
}
