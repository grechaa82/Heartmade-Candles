using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Bot;

internal interface ITelegramUserCache
{
    Result<TelegramUser> GetByChatId (long chatId);

    Result AddOrUpdateUser(TelegramUser user);
}
