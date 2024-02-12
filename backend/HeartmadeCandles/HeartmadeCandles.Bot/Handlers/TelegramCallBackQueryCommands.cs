using HeartmadeCandles.Order.Core.Models;

namespace HeartmadeCandles.Bot.Handlers;

public class TelegramCallBackQueryCommands
{
    public static readonly string CallBackQueryCreatedCommand = "/callback" + OrderStatus.Created.ToString().ToLower();
    public static readonly string CallBackQueryConfirmedCommand = "/callback" + OrderStatus.Confirmed.ToString().ToLower();
    public static readonly string CallBackQueryPlacedCommand = "/callback" + OrderStatus.Placed.ToString().ToLower();
    public static readonly string CallBackQueryPaidCommand = "/callback" + OrderStatus.Paid.ToString().ToLower();
    public static readonly string CallBackQueryInProgressCommand = "/callback" + OrderStatus.InProgress.ToString().ToLower();
    public static readonly string CallBackQueryPackedCommand = "/callback" + OrderStatus.Packed.ToString().ToLower();
    public static readonly string CallBackQueryInDeliveryCommand = "/callback" + OrderStatus.InDelivery.ToString().ToLower();
    public static readonly string CallBackQueryCompletedCommand = "/callback" + OrderStatus.Completed.ToString().ToLower();
    public static readonly string CallBackQueryCancelledCommand = "/callback" + OrderStatus.Cancelled.ToString().ToLower();

    public static readonly string CreatedOrderPreviousCommand = CallBackQueryType.CreatedOrderPrevious.ToString().ToLower();
    public static readonly string CreatedOrderNextCommand = CallBackQueryType.CreatedOrderNext.ToString().ToLower();
    public static readonly string CreatedOrderSelectCommand = CallBackQueryType.CreatedOrderSelect.ToString().ToLower();

    public static readonly string ConfirmedOrderPreviousCommand = CallBackQueryType.ConfirmedOrderPrevious.ToString().ToLower();
    public static readonly string ConfirmedOrderNextCommand = CallBackQueryType.ConfirmedOrderNext.ToString().ToLower();
    public static readonly string ConfirmedOrderSelectCommand = CallBackQueryType.ConfirmedOrderSelect.ToString().ToLower();
}
