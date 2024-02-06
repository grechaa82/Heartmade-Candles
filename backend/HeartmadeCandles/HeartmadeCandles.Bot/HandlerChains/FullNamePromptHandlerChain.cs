using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.HandlerChains;

public class FullNamePromptHandlerChain : HandlerChainBase
{
    public FullNamePromptHandlerChain(
        ITelegramBotClient botClient,
        ITelegramUserCache userCache,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, userCache, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(TelegramCommands.GoToCheckoutCommand) ?? false;

    public async override Task Process(Message message, TelegramUser user)
    {
        var newUser = user.UpdateState(TelegramUserState.AskingFullName);

        _userCache.AddOrUpdateUser(newUser);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                """
                Шаг 1 из 3
                
                !!! В данный момент доставка возможно только Почтой России, приносим свои извинения.

                Отправьте одним сообщение вашу Фамилию Имя и Отчество.

                Пример: Константинопольский Константин Владимирович
                """),
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
