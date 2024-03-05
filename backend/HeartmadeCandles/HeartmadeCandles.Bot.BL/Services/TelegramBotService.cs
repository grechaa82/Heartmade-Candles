using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;

namespace HeartmadeCandles.Bot.BL.Services;

public class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBotRepository _telegramBotRepository;

    public TelegramBotService(ITelegramBotRepository telegramBotRepository)
    {
        _telegramBotRepository = telegramBotRepository;
    }

    public async Task<Result<long[]>> GetChatIdsByRole(TelegramUserRole role)
    {
        return await _telegramBotRepository.GetChatIdsByRole(role);
    }

    public async Task<Result> UpgradeChatRoleToAdmin(long[] newAdminChatIds)
    { 
        return await _telegramBotRepository.UpgradeChatRoleToAdmin(newAdminChatIds);
    } 
}
