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
        try
        {
            var orderText = GetMessageInMarkdownMode(order);

            await _client.SendTextMessageAsync(_chatId, orderText);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private static string GetMessageInMarkdownMode(Core.Models.Order order)
    {
        var message = new StringBuilder();

        message.AppendLine($"Строка конфигурации: {order.ConfiguredCandlesString}");
        message.AppendLine(" ");
        message.AppendLine(
            $"Информация о покупателя: {order.User.FirstName} {order.User.LastName}, {order.User.Phone}, " +
            $"{(string.IsNullOrEmpty(order.User.Email) ? "N/A" : order.User.Email)}");
        message.AppendLine($"Обратная связь: {order.Feedback.TypeFeedback} {order.Feedback.UserName}");
        message.AppendLine(" ");
        message.AppendLine(GetInformationAboutCandles(order.OrderItems));
        message.AppendLine($"Общая количество: {order.TotalQuantity}");
        message.AppendLine($"Общая стоимость: {order.TotalPrice}");

        return message.ToString();
    }

    private static string GetInformationAboutCandles(OrderItem[] candles)
    {
        var message = new StringBuilder();

        for (var i = 0; i < candles.Length; i++)
        {
            message.AppendLine(
                $"Свеча: {candles[i].CandleDetail.Candle.Title}, {candles[i].Quantity} шт, {candles[i].Price} р");
            message.AppendLine($"Количество слоев: {candles[i].CandleDetail.NumberOfLayer.Number}");
            message.AppendLine("Слои: ");
            for (var j = 0; j < candles[i].CandleDetail.LayerColors.Length; j++)
                message.AppendLine($"{j + 1}. {candles[i].CandleDetail.LayerColors[j].Title}");
            message.AppendLine(
                $"Декор: {(candles[i].CandleDetail.Decor == null ? "N/A" : candles[i].CandleDetail.Decor.Title)}");
            message.AppendLine(
                $"Запах: {(candles[i].CandleDetail.Smell == null ? "N/A" : candles[i].CandleDetail.Smell.Title)}");
            message.AppendLine($"Фитиль: {candles[i].CandleDetail.Wick.Title}");
            message.AppendLine(" ");
        }

        return message.ToString();
    }
}