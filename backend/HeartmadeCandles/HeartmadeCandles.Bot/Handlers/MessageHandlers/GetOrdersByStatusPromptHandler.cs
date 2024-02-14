using HeartmadeCandles.Bot.Documents;
using HeartmadeCandles.Bot.ReplyMarkups;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Bot.Handlers.MessageHandlers;

public class GetOrdersByStatusPromptHandler : MessageHandlerBase
{
    public GetOrdersByStatusPromptHandler(
        ITelegramBotClient botClient,
        IMongoDatabase mongoDatabase,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user)
    {
        if (user.Role == TelegramUserRole.Admin && message.Text != null)
        {
            return message.Text.ToLower().Contains(TelegramMessageCommands.GetOrdersByStatusCommand);
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
            replyMarkup: OrderReplyMarkup.GetOrderSelectionMarkupByStatus(),
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
