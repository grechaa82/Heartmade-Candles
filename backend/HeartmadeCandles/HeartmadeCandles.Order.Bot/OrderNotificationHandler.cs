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

        for (var i = 0; i < basketItems.Length; i++)
        {
            message.AppendLine(
                $"Свеча: {basketItems[i].ConfiguredCandle.Candle.Title}, {basketItems[i].Quantity} шт, {basketItems[i].Price} р");
            message.AppendLine($"Количество слоев: {basketItems[i].ConfiguredCandle.NumberOfLayer.Number}");
            message.AppendLine("Слои: ");
            for (var j = 0; j < basketItems[i].ConfiguredCandle.LayerColors.Length; j++)
                message.AppendLine($"{j + 1}. {basketItems[i].ConfiguredCandle.LayerColors[j].Title}");
            message.AppendLine(
                $"Декор: {(basketItems[i].ConfiguredCandle.Decor == null ? "N/A" : basketItems[i].ConfiguredCandle.Decor?.Title)}");
            message.AppendLine(
                $"Запах: {(basketItems[i].ConfiguredCandle.Smell == null ? "N/A" : basketItems[i].ConfiguredCandle.Smell?.Title)}");
            message.AppendLine($"Фитиль: {basketItems[i].ConfiguredCandle.Wick.Title}");
            message.AppendLine(" ");
        }

        return message.ToString();
    }
}