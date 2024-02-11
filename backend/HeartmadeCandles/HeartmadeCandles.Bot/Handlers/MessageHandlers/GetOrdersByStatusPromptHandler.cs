using HeartmadeCandles.Bot.Documents;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Создан", 
                    callbackData: TelegramCallBackQueryCommands.CallBackQueryCreatedCommand),
                InlineKeyboardButton.WithCallbackData(
                    text: "Подтвержден", 
                    callbackData:TelegramCallBackQueryCommands.CallBackQueryConfirmedCommand),
            },
            new []
            {
               InlineKeyboardButton.WithCallbackData(
                   text: "Оформлен", 
                   callbackData: TelegramCallBackQueryCommands.CallBackQueryPlacedCommand),
               InlineKeyboardButton.WithCallbackData(
                   text: "Оплачен", 
                   callbackData: TelegramCallBackQueryCommands.CallBackQueryPaidCommand),
            },
            new []
            {
               InlineKeyboardButton.WithCallbackData(
                   text: "В работе", 
                   callbackData: TelegramCallBackQueryCommands.CallBackQueryInProgressCommand),
               InlineKeyboardButton.WithCallbackData(
                   text: "Упаковывается", 
                   callbackData: TelegramCallBackQueryCommands.CallBackQueryPackedCommand),
            },
            new []
            {
               InlineKeyboardButton.WithCallbackData(
                   text: "Передан в доставку", 
                   callbackData: TelegramCallBackQueryCommands.CallBackQueryInDeliveryCommand),
               InlineKeyboardButton.WithCallbackData(
                   text: "Завершен", 
                   callbackData: TelegramCallBackQueryCommands.CallBackQueryCompletedCommand),
            },
            new []
            {
               InlineKeyboardButton.WithCallbackData(
                   text: "Отменен", 
                   callbackData: TelegramCallBackQueryCommands.CallBackQueryCancelledCommand)
            },
        });

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Выберите нужный статус заказа:",
            replyMarkup: inlineKeyboard,
            parseMode: ParseMode.MarkdownV2);

        return;
    }
}
