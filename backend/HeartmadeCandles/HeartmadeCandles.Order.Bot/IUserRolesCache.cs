using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Order.Bot;

internal interface IUserRolesCache
{
    Result<UserRole> GetUserRole (long chatId);

    Result AddOrUpdateUserRole (long chatId, UserRole role);

    Result<long[]> GetAdminUserIds();
}
