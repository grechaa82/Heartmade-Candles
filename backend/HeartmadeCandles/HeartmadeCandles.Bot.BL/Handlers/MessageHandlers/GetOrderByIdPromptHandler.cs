using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.BL.Utilities;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class GetOrderByIdPromptHandler : MessageHandlerBase
{
    public GetOrderByIdPromptHandler(
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

        return message.Text.ToLower().Contains(MessageCommands.GetOrderByIdCommand);
    }

    public async override Task Process(Message message, TelegramUser user)
    {
        await _telegramBotRepository.UpdateTelegramUserState(
            user.ChatId,
            TelegramUserState.AskingOrderById);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: TelegramMessageFormatter.Format("Введите номер (id) заказа:"),
            messageThreadId: message.MessageThreadId,
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
