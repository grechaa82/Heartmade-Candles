using System.Text;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Telegram.Bot;

namespace HeartmadeCandles.Order.Bot;

public class OrderNotificationHandler : IOrderNotificationHandler
{
    private static readonly string _chatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");
    private static readonly string _token = Environment.GetEnvironmentVariable("VAR_TELEGRAM_API_TOKEN");
    private static readonly TelegramBotClient _client = new(_token);

    public async Task<Result> OnCreateOrder(Core.Models.Order order)
    {
        throw new NotImplementedException();
    }
}