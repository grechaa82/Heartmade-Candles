using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class OrderIdPromptHandler : MessageHandlerBase
{
    public OrderIdPromptHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(TelegramMessageCommands.InputOrderIdCommand) ?? false;

    public async override Task Process(Message message, TelegramUser user)
    {
        await _telegramBotRepository.UpdateTelegramUserState(
            user.ChatId,
            TelegramUserState.AskingOrderId);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Введите номер заказа: ",
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}