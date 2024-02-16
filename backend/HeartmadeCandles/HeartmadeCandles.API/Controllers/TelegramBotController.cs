using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HeartmadeCandles.API.Controllers;

[ApiController]
[Route("api/bot")]
public class TelegramBotController : Controller
{
    private readonly ILogger<TelegramBotController> _logger;
    private readonly ITelegramBotUpdateHandler _telegramBotUpdateHandler;
    private readonly ITelegramBotService _telegramBotService;

    public TelegramBotController(
        ITelegramBotUpdateHandler telegramBotUpdateHandler,
        ITelegramBotService telegramBotService,
        ILogger<TelegramBotController> logger)
    {
        _telegramBotUpdateHandler = telegramBotUpdateHandler;
        _telegramBotService = telegramBotService;
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "Telegram bot was started";
    }

    [HttpPost("update")]
    public async Task Update(Update update)
    {
        await _telegramBotUpdateHandler.Update(update);
    }

    [HttpGet("chat/")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetChatIdsByRole(TelegramUserRole role)
    {
        var result = await _telegramBotService.GetChatIdsByRole(role);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("chat/upgrade")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpgradeChatRoleToAdmin(long[] chatIds)
    {
        var result = await _telegramBotService.UpgradeChatRoleToAdmin(chatIds);
    
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
    
        return Ok();
    }
}