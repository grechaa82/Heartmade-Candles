using CSharpFunctionalExtensions;
using System.Collections.Concurrent;

namespace HeartmadeCandles.Order.Bot;

internal class UserRolesCache : IUserRolesCache
{
    private readonly ConcurrentDictionary<long, UserRole> _userRoles = new ConcurrentDictionary<long, UserRole>();
    private static readonly string _chatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");

    public UserRolesCache()
    {
        AddOrUpdateUserRole(_chatId == null ? 0 : long.Parse(_chatId), UserRole.Admin);
    }

    public Result<UserRole> GetUserRole(long chatId)
    {
        if (_userRoles.ContainsKey(chatId))
        {
            return _userRoles[chatId];
        }
        else
        {
            return UserRole.None;
        }
    }

    public Result AddOrUpdateUserRole(long chatId, UserRole role)
    {
        _userRoles.AddOrUpdate(chatId, role, (key, oldValue) => role);

        return Result.Success();
    }

    public Result<long[]> GetAdminUserIds()
    {
        var adminUserIds = _userRoles.Where(x => x.Value == UserRole.Admin).Select(x => x.Key).ToArray();
        return Result.Success(adminUserIds);
    }
}
