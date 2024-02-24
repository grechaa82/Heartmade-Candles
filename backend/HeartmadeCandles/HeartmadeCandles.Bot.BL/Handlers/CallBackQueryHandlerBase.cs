using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeartmadeCandles.Bot.BL.Handlers;

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

    public async Task<(Maybe<Order.Core.Models.Order[]>, long)> GetOrderByStatusWithTotalOrders(OrderStatus status, int pageSize, int pageIndex = 0)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        return await orderService.GetOrderByStatusWithTotalOrders(status, pageSize, pageIndex);
    }

    public async Task<Result<Order.Core.Models.Order>> GetOrderById(string orderId)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var orderResult = await orderService.GetOrderById(orderId);

        return orderResult;
    }
}
