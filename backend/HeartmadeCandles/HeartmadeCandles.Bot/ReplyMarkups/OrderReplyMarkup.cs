using HeartmadeCandles.Bot.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.ReplyMarkups;

internal class OrderReplyMarkup
{
    public static InlineKeyboardMarkup GetOrderSelectionMarkupByStatus()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Создан",
                    callbackData: $"{TelegramCallBackQueryCommands.CreatedOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Подтвержден",
                    callbackData: $"{TelegramCallBackQueryCommands.ConfirmedOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Оформлен",
                    callbackData: $"{TelegramCallBackQueryCommands.CallBackQueryPlacedCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Оплачен",
                    callbackData: $"{TelegramCallBackQueryCommands.CallBackQueryPaidCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "В работе",
                    callbackData: $"{TelegramCallBackQueryCommands.CallBackQueryInProgressCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Упаковывается",
                    callbackData: $"{TelegramCallBackQueryCommands.CallBackQueryPackedCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Передан в доставку",
                    callbackData: $"{TelegramCallBackQueryCommands.CallBackQueryInDeliveryCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Завершен",
                    callbackData: $"{TelegramCallBackQueryCommands.CallBackQueryCompletedCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Отменен",
                    callbackData: $"{TelegramCallBackQueryCommands.CallBackQueryCancelledCommand}:1"),
            },
        });
    }

    public static InlineKeyboardMarkup GetOrderSelectionMarkup(
        string previousCommands,
        string nextCommands,
        string selectCommands,
        string orderId, 
        int currentPageIndex)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "<",
                    callbackData: $"{previousCommands}:{currentPageIndex - 1}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Выбрать",
                    callbackData: $"{selectCommands}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: ">",
                    callbackData: $"{nextCommands}:{currentPageIndex + 1}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Вернуться к выбору",
                    callbackData: CallBackQueryType.GetOrders.ToString().ToLower()),
            },
        });
    }

    public static InlineKeyboardMarkup GetBackSelectionMarkup()
    {
        return new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData(
                text: "Вернуться к выбору",
                callbackData: CallBackQueryType.GetOrders.ToString().ToLower()),
        });
    }
}
