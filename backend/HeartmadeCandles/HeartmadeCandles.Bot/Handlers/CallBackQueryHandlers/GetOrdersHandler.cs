using HeartmadeCandles.Bot.Documents;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Bot.ReplyMarkups;

namespace HeartmadeCandles.Bot.Handlers.CallBackQueryHandlers;

public class GetOrdersHandler : CallBackQueryHandlerBase
{
    public GetOrdersHandler(
       ITelegramBotClient botClient,
       IMongoDatabase mongoDatabase,
       IServiceScopeFactory serviceScopeFactory)
       : base(botClient, mongoDatabase, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(CallbackQuery callbackQuery, TelegramUser user)
    {
        if (user.Role != TelegramUserRole.Admin || callbackQuery.Data == null)
        {
            return false;
        }

        return callbackQuery.Data.ToLower().Contains(CallBackQueryType.GetOrders.ToString().ToLower());
    }

    public async override Task Process(CallbackQuery callbackQuery, TelegramUser user)
    {
        await _botClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: "Выберите нужный статус заказа:",
            replyMarkup: OrderReplyMarkup.GetOrderSelectionMarkupByStatus(),
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}