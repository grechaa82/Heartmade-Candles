using HeartmadeCandles.Bot;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HeartmadeCandles.API.Controllers;

[ApiController]
[Route("api/bot")]
public class TelegramBotController : Controller
{
    private readonly ILogger<TelegramBotController> _logger;
    private readonly ITelegramBotUpdateHandler _telegramBotService;

    public TelegramBotController(
        ITelegramBotUpdateHandler telegramBotService, 
        ILogger<TelegramBotController> logger)
    {
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
        await _telegramBotService.Update(update);
    }
}