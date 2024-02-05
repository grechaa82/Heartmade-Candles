using HeartmadeCandles.Bot;
using HeartmadeCandles.Order.Bot;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.API.Controllers;

[ApiController]
[Route("api/bot")]
public class TelegramBotController : Controller
{
    private readonly ILogger<TelegramBotController> _logger;
    private readonly ITelegramBotService _telegramBotService;

    public TelegramBotController(
        ITelegramBotService telegramBotService, 
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