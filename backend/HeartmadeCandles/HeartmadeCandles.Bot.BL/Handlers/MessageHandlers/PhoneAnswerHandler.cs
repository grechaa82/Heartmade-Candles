using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Bot.Core;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeartmadeCandles.Bot.BL.Handlers.MessageHandlers;

public class PhoneAnswerHandler : MessageHandlerBase
{
    public PhoneAnswerHandler(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, telegramBotRepository, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingPhone;

    public async override Task Process(Message message, TelegramUser user)
    {
        await _telegramBotRepository.UpdateTelegramUserState(
                user.ChatId,
                TelegramUserState.AskingDeliveryType);

        await ForwardPhoneToAdminAsync(_botClient, message, user);

        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                $"1 - {DeliveryType.Pochta}",
                $"2 - {DeliveryType.Sdek}"
            },
            new KeyboardButton[]
            {
                $"3 - {DeliveryType.Pickup}"
            },
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                """
                Шаг 3 из 4
                
                Выберите тип доставки
                1 - Почта России
                2 - СДЭК
                3 - Самовывоз, Санкт-Петербург, Казанская площадь, 2
                (администратор с вами свяжется для уточнения)

                Укажите цифру или тип доставки, или воспользуйтесь кнопками внизу
                """),
            replyMarkup: replyMarkup, 
            parseMode: ParseMode.MarkdownV2);

        return;
    }

    private async Task ForwardPhoneToAdminAsync(ITelegramBotClient botClient, Message message, TelegramUser user, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: _adminChatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters($"Пользователь {user.UserName} и заказом " +
            $"{user.CurrentOrderId} заполнил свой номер телефона:"),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);

        await botClient.ForwardMessageAsync(
            chatId: _adminChatId,
            fromChatId: user.ChatId,
            messageId: message.MessageId,
            cancellationToken: cancellationToken);
    }

    public class DeliveryType
    {
        public static readonly string Pochta = "Почта России";
        public static readonly string Sdek = "СДЭК";
        public static readonly string Pickup = "Самовывоз";
    }
}
