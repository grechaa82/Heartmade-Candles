using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.HandlerChains;

public class OrderPromptHandlerChain : HandlerChainBase
{
    public OrderPromptHandlerChain(
        ITelegramBotClient botClient,
        ITelegramUserCache userCache,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, userCache, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(TelegramCommands.InputOrderIdCommand) ?? false;

    public async override Task Process(Message message, TelegramUser user)
    {
        var newUser = user.UpdateState(TelegramUserState.AskingOrderId);

        _userCache.AddOrUpdateUser(newUser);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Введите номер заказа: ",
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}