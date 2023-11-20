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
            if (order.Basket == null)
            {
                return Result.Failure("Basket cannot be null");
            }
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

        message.AppendLine($"Строка конфигурации: {order.Basket.FilterString}");
        message.AppendLine(" ");
        message.AppendLine(
            $"Информация о покупателя: {order.User.FirstName} {order.User.LastName}, {order.User.Phone}, " +
            $"{(string.IsNullOrEmpty(order.User.Email) ? "N/A" : order.User.Email)}");
        message.AppendLine($"Обратная связь: {order.Feedback.TypeFeedback} {order.Feedback.UserName}");
        message.AppendLine(" ");
        message.AppendLine(GetInformationAboutCandles(order.Basket.Items));
        message.AppendLine($"Общая количество: {order.Basket.TotalQuantity}");
        message.AppendLine($"Общая стоимость: {order.Basket.TotalPrice}");

        return message.ToString();
    }

    private static string GetInformationAboutCandles(BasketItem[] basketItems)
    {
        var message = new StringBuilder();

        foreach (var item in basketItems)
        {
            message.AppendLine(
                $"Свеча: {item.ConfiguredCandle.Candle.Title}, {item.Quantity} шт, {item.Price} р");
            message.AppendLine($"Количество слоев: {item.ConfiguredCandle.NumberOfLayer.Number}");
            message.AppendLine("Слои: ");
            foreach (var color in item.ConfiguredCandle.LayerColors)
            {
                message.AppendLine($"{Array.IndexOf(item.ConfiguredCandle.LayerColors, color) + 1}. {color.Title}");
            }
            message.AppendLine(
                $"Декор: {(item.ConfiguredCandle.Decor == null ? "N/A" : item.ConfiguredCandle.Decor?.Title)}");
            message.AppendLine(
                $"Запах: {(item.ConfiguredCandle.Smell == null ? "N/A" : item.ConfiguredCandle.Smell?.Title)}");
            message.AppendLine($"Фитиль: {item.ConfiguredCandle.Wick.Title}");
            message.AppendLine(" ");
        }

        return message.ToString();
    }
}