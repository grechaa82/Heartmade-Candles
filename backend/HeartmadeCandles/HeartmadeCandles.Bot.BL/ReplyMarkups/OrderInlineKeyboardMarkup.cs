﻿using HeartmadeCandles.Bot.BL.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.ReplyMarkups;

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
                    callbackData: $"{TelegramCallBackQueryCommands.CreatedOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Подтвержден",
                    callbackData: $"{TelegramCallBackQueryCommands.ConfirmedOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Оформлен",
                    callbackData: $"{TelegramCallBackQueryCommands.PlacedOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Оплачен",
                    callbackData: $"{TelegramCallBackQueryCommands.PaidOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "В работе",
                    callbackData: $"{TelegramCallBackQueryCommands.InProgressOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Упаковывается",
                    callbackData: $"{TelegramCallBackQueryCommands.PackedOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Передан в доставку",
                    callbackData: $"{TelegramCallBackQueryCommands.InDeliveryOrderNextCommand}:1"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Завершен",
                    callbackData: $"{TelegramCallBackQueryCommands.CompletedOrderNextCommand}:1"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Отменен",
                    callbackData: $"{TelegramCallBackQueryCommands.CancelledOrderNextCommand}:1"),
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
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateOrderStatusCommand}:{orderId}"),
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
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToCreatedCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Подтвержден",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToConfirmedCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Оформлен",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToPlacedCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Оплачен",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToPaidCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "В работе",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToInProgressCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Упаковывается",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToPackedCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Передан в доставку",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToInDeliveryCommand}:{orderId}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Завершен",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToCompletedCommand}:{orderId}"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Отменен",
                    callbackData: $"{TelegramCallBackQueryCommands.UpdateToCancelledCommand}:{orderId}"),
            },
        });
    }
}
