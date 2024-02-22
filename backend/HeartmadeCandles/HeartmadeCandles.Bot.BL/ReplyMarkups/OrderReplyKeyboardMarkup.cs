using HeartmadeCandles.Bot.BL.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.ReplyMarkups;

internal class OrderReplyKeyboardMarkup
{
    public static ReplyKeyboardMarkup GetOrderCommands()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Показать заказы {TelegramMessageCommands.GetOrderInfoCommand}",
                $"Статус заказ {TelegramMessageCommands.GetOrderStatusCommand}"
            },
            new KeyboardButton[]
            {
                $"Оформить заказ {TelegramMessageCommands.GoToCheckoutCommand}"
            },
        })
        {
            ResizeKeyboard = true
        };
    }

    public static ReplyKeyboardMarkup GetOrderGoToCheckoutCommand()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Оформить заказ {TelegramMessageCommands.GoToCheckoutCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };
    }
}
