using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using static System.Net.Mime.MediaTypeNames;
using CSharpFunctionalExtensions;
using HeartmadeCandles.Order.Core.Interfaces;

namespace HeartmadeCandles.Bot.HandlerChains;

public class OrderAnswerHandlerChain : HandlerChainBase
{
    public OrderAnswerHandlerChain(
        ITelegramBotClient botClient,
        ITelegramUserCache userCache,
        IServiceScopeFactory serviceScopeFactory)
        : base(botClient, userCache, serviceScopeFactory)
    {
    }

    public override bool ShouldHandleUpdate(Message message, TelegramUser user) =>
        user.State == TelegramUserState.AskingOrderId;

    public async override Task Process(Message message, TelegramUser user)
    {
        var orderResult = await CheckOrderAsync(message.Text ?? string.Empty);

        if (orderResult.IsFailure)
        {
            var newUser = user.UpdateState(TelegramUserState.OrderNotExist);
            _userCache.AddOrUpdateUser(newUser);

            await SendOrderProcessingErrorMessage(_botClient, message.Chat.Id);

            return;
        }
        else
        {
            var newUser = new TelegramUser(
                userId: user.UserId,
                chatId: user.ChatId,
                userName: user.UserName,
                firstName: user.FirstName,
                lastName: user.LastName,
                currentOrderId: message.Text,
                state: TelegramUserState.OrderExist,
                role: user.Role);
            _userCache.AddOrUpdateUser(newUser);

            await SendInfoAboutCommandsAsync(_botClient, message.Chat.Id);

            return;
        }
    }

    private async Task SendInfoAboutCommandsAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Вам доступны команды: 
                    
                {TelegramCommands.GetOrderInfoCommand} - узнать информацию о заказе
                {TelegramCommands.GetOrderStatusCommand} - узнать текущий статус заказа
                {TelegramCommands.GoToCheckoutCommand} - оформить заказ
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task SendOrderProcessingErrorMessage(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken = default)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: OrderInfoFormatter.EscapeSpecialCharacters(
                $"""
                Возникла проблема с вашим заказом. Мы не смогли его найти. 
                
                Вы можете:
                - Попробовать ввести номер заказа еще раз {TelegramCommands.InputOrderIdCommand}
                - Создать новый заказ на нашем сайте 4fass.ru
                """),
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken);
    }

    private async Task<Result> CheckOrderAsync(string orderId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            return Result.Failure(orderResult.Error);
        }
        return Result.Success();
    }
}
