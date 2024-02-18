using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Bot.Core;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class GetChatIdHandler : MessageHandlerBase
{
    public GetChatIdHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        message.Text?.ToLower().Contains(TelegramMessageCommands.GetChatIdCommand) ?? false;

    public async override Task Process(Message message, TelegramUser user)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: OrderInfoFormatter.EscapeSpecialCharacters(message.Chat.Id.ToString()),
            messageThreadId: message.MessageThreadId,
            parseMode: ParseMode.MarkdownV2);
    }
}
