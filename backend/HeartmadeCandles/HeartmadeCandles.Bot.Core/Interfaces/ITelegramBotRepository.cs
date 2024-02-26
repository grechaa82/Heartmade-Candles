using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Core.Models;

namespace HeartmadeCandles.Bot.Core.Interfaces;

public interface ITelegramBotRepository
{
    Task<Maybe<TelegramUser>> GetTelegramUserByChatId(long chatId);

    Task<Result<string>> CreateTelegramUser(TelegramUser user);

    Task<Result> UpdateOrderId(long chatId, string orderId);

    Task<Result> UpdateTelegramUserState(long chatId, TelegramUserState state);

    Task<Result<long[]>> GetChatIdsByRole(TelegramUserRole role);

    Task<Result> UpgradeChatRoleToAdmin(long[] newAdminChatIds);
}
