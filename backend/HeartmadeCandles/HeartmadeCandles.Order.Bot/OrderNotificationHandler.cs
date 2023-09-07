using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace HeartmadeCandles.Order.Bot
{
    public class OrderNotificationHandler : IOrderNotificationHandler
    {
        private static readonly string chatId = "502372730";
        private static readonly string token = "6005658712:AAHRoDoomqxZ_efYr3wBAJEjYf9TP8wlvPg";
        private static TelegramBotClient client = new TelegramBotClient(token);

        public async Task<Result> OnCreateOrder(Core.Models.Order order)
        {
            try
            {
                string orderText = GetMessageInMarkdownMode(order);

                await client.SendTextMessageAsync(chatId, orderText, parseMode: ParseMode.Markdown);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        private static string GetMessageInMarkdownMode(Core.Models.Order order)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine($"Строка конфигурации: {order.ConfiguredCandlesString}");
            message.AppendLine(" ");
            message.AppendLine($"Информация о покупателя: {order.User.FirstName} {order.User.LastName}, {order.User.Phone}, " +
                $"{(string.IsNullOrEmpty(order.User.Email) ? "N/A" : order.User.Email)}");
            message.AppendLine($"Обратная связь: {order.Feedback.TypeFeedback} {order.Feedback.UserName}");
            message.AppendLine(" ");
            message.AppendLine(GetInformationAboutCandles(order.Candles));
            message.AppendLine($"Общая количество: {order.GetTotalQuantity()}");
            message.AppendLine($"Общая стоимость: {order.GetTotalPrice()}");

            return message.ToString();
        }

        private static string GetInformationAboutCandles(CandleDetailWithQuantityAndPrice[] candles)
        {
            StringBuilder message = new StringBuilder();

            for (var i = 0; i < candles.Length; i++)
            {
                message.AppendLine($"Свеча: {candles[i].CandleDetail.Candle.Title}, {candles[i].Quantity} шт, {candles[i].Price} р");
                message.AppendLine($"Количество слоев: {candles[i].CandleDetail.NumberOfLayer.Number}");
                message.AppendLine($"Слои: ");
                for (var j = 0; j < candles[i].CandleDetail.LayerColors.Length; j++)
                {
                    message.AppendLine($"{j + 1}. {candles[i].CandleDetail.LayerColors[j].Title}");
                }
                message.AppendLine($"Декор: {(candles[i].CandleDetail.Decor == null ? "N/A" : candles[i].CandleDetail.Decor.Title)}");
                message.AppendLine($"Запах: {(candles[i].CandleDetail.Smell == null ? "N/A" : candles[i].CandleDetail.Smell.Title)}");
                message.AppendLine($"Фитиль: {candles[i].CandleDetail.Wick.Title}");
                message.AppendLine(" ");
            }

            return message.ToString();
        }

    }
}