using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot;

public interface ITelegramBotUpdateHandler
{
    Task Update(Update update);
}
