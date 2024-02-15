using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class FullNamePromptHandler : MessageHandlerBase
{
    public FullNamePromptHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user)
    {
        if (user.State == TelegramUserState.OrderExist && message.Text != null)
        {
            return message.Text.ToLower().Contains(TelegramMessageCommands.GoToCheckoutCommand);
        }
        else
        {
            return false;
        }
    }


    public async override Task Process(Message message, TelegramUser user)
    {
        await _telegramBotRepository.UpdateTelegramUserState(
                user.ChatId,
                TelegramUserState.AskingFullName);

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
