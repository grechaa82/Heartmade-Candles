using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.HandlerChains;

public class PhoneAnswerHandlerChain : HandlerChainBase
{
    public PhoneAnswerHandlerChain(
        ITelegramBotClient botClient,
        ITelegramUserCache userCache,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, userCache, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingPhone;

    public async override Task Process(Message message, TelegramUser user)
    {
        var newUser = user.UpdateState(TelegramUserState.AskingAddress);

        _userCache.AddOrUpdateUser(newUser);

        await ForwardPhoneToAdminAsync(_botClient, message, user);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                """
                Шаг 3 из 3
                
                Введите свой адрес, куда доставить посылку.
                
                Пример: Санкт-Петербург, Казанская площадь, дом 4, кв. 12
                """),
            parseMode: ParseMode.MarkdownV2);

        return;
    }

    private async Task ForwardPhoneToAdminAsync(ITelegramBotClient botClient, Message message, TelegramUser user, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом " +
            $"{user.CurrentOrderId} заполнил свой номер телефона:"),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        await botClient.ForwardMessageAsync(
            chatId: _adminChatId,
            fromChatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken);
    }
}
