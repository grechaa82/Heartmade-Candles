using HeartmadeCandles.Bot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using HeartmadeCandles.Bot.ReplyMarkups;
using HeartmadeCandles.Bot.Core;

namespace HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;

public class GetOrdersHandler : CallBackQueryHandlerBase
{
    public GetOrdersHandler(
       ITelegramBotClient botClient,
       ITelegramBotRepository telegramBotRepository,
       IServiceScopeFactory serviceScopeFactory)
       : base(botClient, telegramBotRepository, serviceScopeFactory)
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
            replyMarkup: OrderInlineKeyboardMarkup.GetOrderSelectionMarkupByStatus(),
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}