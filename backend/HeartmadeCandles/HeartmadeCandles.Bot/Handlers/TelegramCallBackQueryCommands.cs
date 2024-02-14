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

    public static readonly string GetOrderIdCommand = CallBackQueryType.GetOrderId.ToString().ToLower();

    public static readonly string CreatedOrderPreviousCommand = CallBackQueryType.CreatedOrderPrevious.ToString().ToLower();
    public static readonly string CreatedOrderNextCommand = CallBackQueryType.CreatedOrderNext.ToString().ToLower();
    public static readonly string CreatedOrderSelectCommand = CallBackQueryType.CreatedOrderSelect.ToString().ToLower();

    public static readonly string ConfirmedOrderPreviousCommand = CallBackQueryType.ConfirmedOrderPrevious.ToString().ToLower();
    public static readonly string ConfirmedOrderNextCommand = CallBackQueryType.ConfirmedOrderNext.ToString().ToLower();
    public static readonly string ConfirmedOrderSelectCommand = CallBackQueryType.ConfirmedOrderSelect.ToString().ToLower();

    public static readonly string PlacedOrderPreviousCommand = CallBackQueryType.PlacedOrderPrevious.ToString().ToLower();
    public static readonly string PlacedOrderNextCommand = CallBackQueryType.PlacedOrderNext.ToString().ToLower();
    public static readonly string PlacedOrderSelectCommand = CallBackQueryType.PlacedOrderSelect.ToString().ToLower();

    public static readonly string PaidOrderPreviousCommand = CallBackQueryType.PaidOrderPrevious.ToString().ToLower();
    public static readonly string PaidOrderNextCommand = CallBackQueryType.PaidOrderNext.ToString().ToLower();
    public static readonly string PaidOrderSelectCommand = CallBackQueryType.PaidOrderSelect.ToString().ToLower();

    public static readonly string InProgressOrderPreviousCommand = CallBackQueryType.InProgressOrderPrevious.ToString().ToLower();
    public static readonly string InProgressOrderNextCommand = CallBackQueryType.InProgressOrderNext.ToString().ToLower();
    public static readonly string InProgressOrderSelectCommand = CallBackQueryType.InProgressOrderSelect.ToString().ToLower();

    public static readonly string PackedOrderPreviousCommand = CallBackQueryType.PackedOrderPrevious.ToString().ToLower();
    public static readonly string PackedOrderNextCommand = CallBackQueryType.PackedOrderNext.ToString().ToLower();
    public static readonly string PackedOrderSelectCommand = CallBackQueryType.PackedOrderSelect.ToString().ToLower();

    public static readonly string InDeliveryOrderPreviousCommand = CallBackQueryType.InDeliveryOrderPrevious.ToString().ToLower();
    public static readonly string InDeliveryOrderNextCommand = CallBackQueryType.InDeliveryOrderNext.ToString().ToLower();
    public static readonly string InDeliveryOrderSelectCommand = CallBackQueryType.InDeliveryOrderSelect.ToString().ToLower();

    public static readonly string CompletedOrderPreviousCommand = CallBackQueryType.CompletedOrderPrevious.ToString().ToLower();
    public static readonly string CompletedOrderNextCommand = CallBackQueryType.CompletedOrderNext.ToString().ToLower();
    public static readonly string CompletedOrderSelectCommand = CallBackQueryType.CompletedOrderSelect.ToString().ToLower();

    public static readonly string CancelledOrderPreviousCommand = CallBackQueryType.CancelledOrderPrevious.ToString().ToLower();
    public static readonly string CancelledOrderNextCommand = CallBackQueryType.CancelledOrderNext.ToString().ToLower();
    public static readonly string CancelledOrderSelectCommand = CallBackQueryType.CancelledOrderSelect.ToString().ToLower();
}
