using HeartmadeCandles.BusinessLogic.Services;
using HeartmadeCandles.Core.Interfaces;
using HeartmadeCandles.Core.Models;
using Newtonsoft.Json;
using Telegram.Bot;

namespace HeartmadeCandles.Bot
{
    public class OrderCreateHandler : IOrderCreateHandler
    {
        private static readonly string chatId = "502372730";
        private static readonly string token = "6005658712:AAHRoDoomqxZ_efYr3wBAJEjYf9TP8wlvPg";
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