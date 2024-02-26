using HeartmadeCandles.Bot.BL.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Utilities.ReplyMarkups;

internal class OrderReplyKeyboardMarkup
{
    public static ReplyKeyboardMarkup GetOrderCommands()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"Показать заказы {MessageCommands.GetOrderInfoCommand}",
                $"Статус заказ {MessageCommands.GetOrderStatusCommand}"
            },
            new KeyboardButton[]
            {
                $"Оформить заказ {MessageCommands.GoToCheckoutCommand}"
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
                $"Оформить заказ {MessageCommands.GoToCheckoutCommand}",
            }
        })
        {
            ResizeKeyboard = true
        };
    }
}
