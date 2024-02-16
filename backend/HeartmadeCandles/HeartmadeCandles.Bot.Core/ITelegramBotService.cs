using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Core.Models;

namespace HeartmadeCandles.Bot.Core;

public interface ITelegramBotService
{
    Task<Result<long[]>> GetChatIdsByRole(TelegramUserRole role);

    Task<Result> UpgradeChatRoleToAdmin(long[] newAdminChatIds);
}
