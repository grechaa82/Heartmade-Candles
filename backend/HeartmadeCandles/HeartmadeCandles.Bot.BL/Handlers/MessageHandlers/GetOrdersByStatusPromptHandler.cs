using HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class GetOrdersByStatusPromptHandler : MessageHandlerBase
{
    public GetOrdersByStatusPromptHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user)
    {
        if (user.Role == TelegramUserRole.Admin && message.Text != null)
        {
            return message.Text.ToLower().Contains(MessageCommands.GetOrdersByStatusCommand);
        }
        else
        {
            return false;
        }
    }

    public async override Task Process(Message message, TelegramUser user)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Выберите нужный статус заказа:",
            messageThreadId: message.MessageThreadId,
            replyMarkup: OrderInlineKeyboardMarkup.GetOrderSelectionMarkupByStatus(),
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
