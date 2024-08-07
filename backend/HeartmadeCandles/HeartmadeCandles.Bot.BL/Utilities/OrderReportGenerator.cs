﻿using HeartmadeCandles.Bot.Core.Models;
using System.Text;

namespace HeartmadeCandles.Bot.BL.Utilities;

public class OrderReportGenerator
{
    public static string GenerateReport(Core.Models.Order order)
    {
        var message = new StringBuilder();

        message.AppendLine($"Номер заказа: {order.Id}");
        message.AppendLine($"Строка конфигурации: {order.Basket.FilterString}");
        message.AppendLine(" ");
        message.AppendLine($"Создана: {order.CreatedAt}, обновлена: {order.UpdatedAt}");
        message.AppendLine(" ");
        message.AppendLine($"Обратная связь: {order.Feedback.TypeFeedback} {order.Feedback.UserName}");
        message.AppendLine(" ");
        message.AppendLine(GenerateCandlesReported(order.Basket.Items));
        message.AppendLine($"Общая количество: {order.Basket.TotalQuantity}");
        message.AppendLine($"Общая стоимость: {order.Basket.TotalPrice}");

        return TelegramMessageFormatter.Format(message.ToString());
    }

    private static string GenerateCandlesReported(BasketItem[] basketItems)
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

    public static string GeneratePreviewReport(Core.Models.Order order)
    {
        var message = new StringBuilder();

        message.AppendLine($"Номер заказа: {order.Id}");
        message.AppendLine($"Строка конфигурации: {order.Basket.FilterString}");
        message.AppendLine(" ");
        message.AppendLine($"Создана: {order.CreatedAt}, обновлена: {order.UpdatedAt}");
        message.AppendLine(" ");
        message.AppendLine(GenerateCandlesPreviewReported(order.Basket.Items));
        message.AppendLine($"Свечей: {order.Basket.TotalQuantity}");
        message.AppendLine($"Итого: {order.Basket.TotalPrice}");

        return TelegramMessageFormatter.Format(message.ToString());
    }

    private static string GenerateCandlesPreviewReported(BasketItem[] basketItems)
    {
        var message = new StringBuilder();

        foreach (var item in basketItems)
        {
            message.AppendLine(
                $"Свеча: {item.ConfiguredCandle.Candle.Title}, {item.Quantity} шт, {item.Price} р");
            message.AppendLine(" ");
        }

        return message.ToString();
    }
}
