using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Core.Interfaces;
using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Order.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.BL.Handlers.CallBackQueryHandlers;

public abstract class CallBackQueryHandlerBase
{
    protected readonly ITelegramBotClient _botClient;
    protected readonly ITelegramBotRepository _telegramBotRepository;
    protected readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly string _adminChatId = Environment.GetEnvironmentVariable("VAR_TELEGRAM_CHAT_ID");

    protected CallBackQueryHandlerBase(
        ITelegramBotClient botClient,
        ITelegramBotRepository telegramBotRepository,
        IServiceScopeFactory serviceScopeFactory)
    {
        _botClient = botClient;
        _telegramBotRepository = telegramBotRepository;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public abstract bool ShouldHandleUpdate(CallbackQuery callbackQuery, TelegramUser user);

    public abstract Task Process(CallbackQuery callbackQuery, TelegramUser user);

    public async Task<(Maybe<Core.Models.Order[]>, long)> GetOrderByStatusWithTotalOrders(OrderStatus status, int pageSize, int pageIndex = 0)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderStatus = BotMapping.MapBotOrderStatusToOrderOrderStatus(status);

        var (orderMaybe, totalOrders) = await orderService.GetOrderByStatusWithTotalOrders(orderStatus, pageSize, pageIndex);

        if (!orderMaybe.HasValue)
        {
            return (Maybe.None, totalOrders);
        }

        return (BotMapping.MapOrderToBotOrder(orderMaybe.Value), totalOrders);
    }

    public async Task<Result<Core.Models.Order>> GetOrderById(string orderId)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        if (orderResult.IsFailure)
        {
            return Result.Failure<Core.Models.Order>(orderResult.Error);
        }

        return BotMapping.MapOrderToBotOrder(orderResult.Value);
    }
}
