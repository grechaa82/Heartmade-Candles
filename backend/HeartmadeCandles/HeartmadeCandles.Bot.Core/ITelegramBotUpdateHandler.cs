using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.Core;

public interface ITelegramBotUpdateHandler
{
    Task Update(Update update);
}
