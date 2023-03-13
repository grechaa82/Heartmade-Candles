using HeartmadeCandles.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.API.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string chatId = "502372730";
        public BotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NewOrder(Order order)
        {
            string orderText = JsonConvert.SerializeObject(order);
            //(string)JsonConvert.DeserializeObject(order.ToString());

            var token = _configuration.GetSection("Token").Value;

            TelegramBotClient client = new TelegramBotClient(token);

            await client.SendTextMessageAsync(chatId, orderText);

            return Ok();
        }
    }
}