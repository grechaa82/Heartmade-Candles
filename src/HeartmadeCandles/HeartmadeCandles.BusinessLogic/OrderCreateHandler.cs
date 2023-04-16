using HeartmadeCandles.BusinessLogic.Services;
using HeartmadeCandles.Core.Interfaces;
using HeartmadeCandles.Core.Models;
using Newtonsoft.Json;
using Telegram.Bot;

namespace HeartmadeCandles.Bot
{
    public class OrderCreateHandler : IOrderCreateHandler
    {
        private static readonly string chatId = "N-chatId";
        private static readonly string token = "N-token";
        private static TelegramBotClient client = new TelegramBotClient(token);

        public OrderCreateHandler()
        {
            var CandleConstructorService = new CandleConstructorService();
            CandleConstructorService.OrderCreated += OnOrderCreated;
        }

        public async void OnOrderCreated(Order order)
        {
            string orderText = JsonConvert.SerializeObject(order);

            await client.SendTextMessageAsync(chatId, orderText);
        }
    }
}
