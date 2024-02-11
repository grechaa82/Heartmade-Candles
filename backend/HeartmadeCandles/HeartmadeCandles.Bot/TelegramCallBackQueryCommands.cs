using HeartmadeCandles.Order.Core.Models;
using MongoDB.Driver;

namespace HeartmadeCandles.Bot;

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

    public static OrderStatus GetOrderStatusFromCommand(string command)
    {
        string suffix = command.Replace("/callback", "").ToLower();

        return suffix switch
        {
            "created" => OrderStatus.Created,
            "confirmed" => OrderStatus.Confirmed,
            "placed" => OrderStatus.Placed,
            "paid" => OrderStatus.Paid,
            "inprogress" => OrderStatus.InProgress,
            "packed" => OrderStatus.Packed,
            "indelivery" => OrderStatus.InDelivery,
            "completed" => OrderStatus.Completed,
            "cancelled" => OrderStatus.Cancelled,
        };
    }
}
