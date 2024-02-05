using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Bot;

public interface ITelegramUserCache
{
    Result<TelegramUser> GetByChatId (long chatId);

    Result AddOrUpdateUser(TelegramUser user);
}
