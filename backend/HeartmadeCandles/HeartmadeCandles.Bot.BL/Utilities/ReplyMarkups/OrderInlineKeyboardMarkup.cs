using HeartmadeCandles.Bot.BL.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;

internal class OrderInlineKeyboardMarkup
{
    public static InlineKeyboardMarkup GetOrderSelectionMarkupByStatus()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Создан",
                    callbackData: $"{CallBackQueryCommands.CreatedOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Подтвержден",
                    callbackData: $"{CallBackQueryCommands.ConfirmedOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Оформлен",
                    callbackData: $"{CallBackQueryCommands.PlacedOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Оплачен",
                    callbackData: $"{CallBackQueryCommands.PaidOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "В работе",
                    callbackData: $"{CallBackQueryCommands.InProgressOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Упаковывается",
                    callbackData: $"{CallBackQueryCommands.PackedOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Передан в доставку",
                    callbackData: $"{CallBackQueryCommands.InDeliveryOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Завершен",
                    callbackData: $"{CallBackQueryCommands.CompletedOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Отменен",
                    callbackData: $"{CallBackQueryCommands.CancelledOrderNextCommand}:1"),
            },
        });
    }

    public static InlineKeyboardMarkup GetOrderSelectionMarkup(
        string previousCommands,
        string nextCommands,
        string selectCommands,
        string orderId,
        int currentPageIndex,
        long totalOrders)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "⏮",
                    callbackData: $"{previousCommands}:{1}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "⬅️",
                    callbackData: $"{previousCommands}:{currentPageIndex - 1}"),
                InlineKeyboardButton.WithCallbackData(
                    text: $"{currentPageIndex}/{totalOrders}",
                    callbackData: $"{selectCommands}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "➡️",
                    callbackData: $"{nextCommands}:{(currentPageIndex + 1 > totalOrders
                        ? currentPageIndex
                        : currentPageIndex + 1)}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "⏭",
                    callbackData: $"{nextCommands}:{totalOrders}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Узнать больше",
                    callbackData: $"{selectCommands}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Номер заказ",
                    callbackData: $"{CallBackQueryType.GetOrderId}:{orderId}"),
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

    public static InlineKeyboardMarkup GetMarkupOfSelectedOrder(string orderId)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Номер заказ",
                    callbackData: $"{CallBackQueryType.GetOrderId}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Обновить статус",
                    callbackData: $"{CallBackQueryCommands.UpdateOrderStatusCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Вернуться к выбору",
                    callbackData: CallBackQueryType.GetOrders.ToString().ToLower()),
            },
        });
    }

    public static InlineKeyboardMarkup GetMarkupForSelectingNewOrderStatus(string orderId)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Создан",
                    callbackData: $"{CallBackQueryCommands.UpdateToCreatedCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Подтвержден",
                    callbackData: $"{CallBackQueryCommands.UpdateToConfirmedCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Оформлен",
                    callbackData: $"{CallBackQueryCommands.UpdateToPlacedCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Оплачен",
                    callbackData: $"{CallBackQueryCommands.UpdateToPaidCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "В работе",
                    callbackData: $"{CallBackQueryCommands.UpdateToInProgressCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Упаковывается",
                    callbackData: $"{CallBackQueryCommands.UpdateToPackedCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Передан в доставку",
                    callbackData: $"{CallBackQueryCommands.UpdateToInDeliveryCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Завершен",
                    callbackData: $"{CallBackQueryCommands.UpdateToCompletedCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Отменен",
                    callbackData: $"{CallBackQueryCommands.UpdateToCancelledCommand}:{orderId}"),
            },
        });
    }
}
