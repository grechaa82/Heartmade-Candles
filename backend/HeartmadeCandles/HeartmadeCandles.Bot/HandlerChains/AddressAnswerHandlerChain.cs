using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.HandlerChains;

public class AddressAnswerHandlerChain : HandlerChainBase
{
    public AddressAnswerHandlerChain(
        ITelegramBotClient botClient,
        ITelegramUserCache userCache,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, userCache, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingAddress;

    public async override Task Process(Message message, TelegramUser user)
    {
        var newUser = user.UpdateState(TelegramUserState.OrderExist);
        
        _userCache.AddOrUpdateUser(newUser);

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
                Вы Можете отслеживать статус заказа используя команду {TelegramCommands.GetOrderStatusCommand}
                
                Если возникнут сложности он с вами свяжется.
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }
}
