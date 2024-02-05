using CSharpFunctionalExtensions;
using System.Collections.Concurrent;

namespace HeartmadeCandles.Bot;

public class TelegramUserCache : ITelegramUserCache
{
    private readonly ConcurrentDictionary<long, TelegramUser> _userCache = new ConcurrentDictionary<long, TelegramUser>();

    public Result<TelegramUser> GetByChatId(long chatId)
    {
        if (_userCache.TryGetValue(chatId, out var user))
        {
            return Result.Success(user);
        }
        else
        {
            return Result.Failure<TelegramUser>("User not found");
        }
    }

    public Result AddOrUpdateUser(TelegramUser user)
    {
        _userCache.AddOrUpdate(user.ChatId, user, (_, oldValue) => user);

        return Result.Success();
    }
}
